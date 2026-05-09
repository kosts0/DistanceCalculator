using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using DistanceCalculator.Dtos;
using DistanceCalculator.Exceptions;
using DistanceCalculator.Options;
using Microsoft.Extensions.Options;

namespace DistanceCalculator.Services;

public class TwoGisDistanceCalculator : IDistanceCalculator
{
    private const string EndpointPath = "/get_dist_matrix";
    private static readonly TimeSpan HttpTimeout = TimeSpan.FromSeconds(10);

    private readonly HttpClient _httpClient;
    private readonly TwoGisOptions _options;
    private readonly ILogger<TwoGisDistanceCalculator> _logger;

    public TwoGisDistanceCalculator(
        HttpClient httpClient,
        IOptions<TwoGisOptions> options,
        ILogger<TwoGisDistanceCalculator> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_options.EndpointHost);
        _httpClient.Timeout = HttpTimeout;
    }

    public async Task<double> CalculateAsync(DistanceRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new IntegrationException("2GIS API key is not configured. Please set TwoGis:ApiKey in configuration.", "API_KEY_MISSING");
        }

        var requestDto = new TwoGisRequest(
            new[]
            {
                new RequestPoint(request.PointA.Latitude, request.PointA.Longitude),
                new RequestPoint(request.PointB.Latitude, request.PointB.Longitude)
            },
            new[] { 0 },
            new[] { 1 });

        var jsonContent = JsonSerializer.Serialize(requestDto, JsonOptions);
        var requestMessage = BuildRequestMessage(jsonContent);

        _logger.LogInformation(
            "Requesting distance from 2GIS for points ({PointALat}, {PointALon}) and ({PointBLat}, {PointBLon})",
            request.PointA.Latitude, request.PointA.Longitude,
            request.PointB.Latitude, request.PointB.Longitude);

        try
        {
            using var httpResponse = await _httpClient.SendAsync(requestMessage, cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError(
                    "2GIS API returned error. Status: {StatusCode}, Body: {ErrorBody}",
                    httpResponse.StatusCode, errorBody);

                throw new IntegrationException(
                    $"2GIS API error: {httpResponse.StatusCode} - {errorBody}",
                    "EXTERNAL_API_ERROR");
            }

            var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonSerializer.Deserialize<TwoGisResponse>(responseBody, JsonOptions)
                ?? throw new IntegrationException("Failed to deserialize 2GIS response", "DESERIALIZATION_ERROR");

            if (response.Routes == null || response.Routes.Length == 0)
            {
                throw new IntegrationException("2GIS returned no routes in response", "NO_ROUTES");
            }

            var route = response.Routes[0];
            _logger.LogInformation(
                "2GIS returned route status: {Status}, distance: {Distance}m, duration: {Duration}s",
                route.Status, route.Distance, route.Duration);

            if (!string.Equals(route.Status, "OK", StringComparison.OrdinalIgnoreCase))
            {
                throw new IntegrationException($"2GIS route calculation failed with status: {route.Status}", "ROUTE_STATUS_FAILED");
            }

            return route.Distance / 1000.0;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request to 2GIS failed");
            throw new IntegrationException($"Failed to communicate with 2GIS API: {ex.Message}", ex, "HTTP_ERROR");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "2GIS API request was cancelled");
            throw new IntegrationException("Request to 2GIS API was cancelled", ex, "REQUEST_CANCELLED");
        }
    }

    private HttpRequestMessage BuildRequestMessage(string jsonContent)
    {
        var uriBuilder = new UriBuilder($"{_options.EndpointHost.TrimEnd('/')}{EndpointPath}")
        {
            Query = $"key={Uri.EscapeDataString(_options.ApiKey)}&version={_options.Version}"
        };

        var request = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri);
        request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        return request;
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    private sealed record TwoGisRequest(
        [property: JsonPropertyName("points")] RequestPoint[] Points,
        [property: JsonPropertyName("sources")] int[] Sources,
        [property: JsonPropertyName("targets")] int[] Targets);

    private sealed record RequestPoint(
        [property: JsonPropertyName("lat")] double Lat,
        [property: JsonPropertyName("lon")] double Lon);

    private sealed record TwoGisResponse(
        [property: JsonPropertyName("routes")] Route[]? Routes);

    private sealed record Route(
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("source_id")] int SourceId,
        [property: JsonPropertyName("target_id")] int TargetId,
        [property: JsonPropertyName("distance")] int Distance,
        [property: JsonPropertyName("duration")] int Duration,
        [property: JsonPropertyName("reliability")] double? Reliability);
}