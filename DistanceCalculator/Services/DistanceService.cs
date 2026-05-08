using DistanceCalculator.Dtos;

namespace DistanceCalculator.Services;

public class DistanceService : IDistanceService
{
    private readonly IDistanceCalculator _calculator;
    private readonly ILogger<DistanceService> _logger;

    public DistanceService(IDistanceCalculator calculator, ILogger<DistanceService> logger)
    {
        _calculator = calculator;
        _logger = logger;
    }

    public Task<DistanceResponse> CalculateDistanceAsync(DistanceRequest request)
    {
        _logger.LogInformation(
            "Calculating distance between ({PointALat}, {PointALon}) and ({PointBLat}, {PointBLon})",
            request.PointA.Latitude, request.PointA.Longitude,
            request.PointB.Latitude, request.PointB.Longitude);

        var distance = _calculator.Calculate(request);

        _logger.LogInformation("Calculated distance: {Distance} km", distance);

        return Task.FromResult(new DistanceResponse(distance));
    }
}
