using DistanceCalculator.Dtos;
using DistanceCalculator.Enums;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Services;

public class DistanceService : IDistanceService
{
    private readonly ICalculatorFactory _calculatorFactory;
    private readonly ILogger<DistanceService> _logger;

    public DistanceService(ICalculatorFactory calculatorFactory, ILogger<DistanceService> logger)
    {
        _calculatorFactory = calculatorFactory;
        _logger = logger;
    }

    public async Task<(double Distance, CalculatorType CalculatorType)> CalculateDistanceAsync(
        DistanceRequest request,
        CalculatorType calculatorType = CalculatorType.Haversine,
        CancellationToken cancellationToken = default)
    {
        var calculator = _calculatorFactory.GetCalculator(calculatorType);

        _logger.LogInformation(
            "Calculating distance between ({PointALat}, {PointALon}) and ({PointBLat}, {PointBLon}) using {CalculatorType}",
            request.PointA.Latitude, request.PointA.Longitude,
            request.PointB.Latitude, request.PointB.Longitude,
            calculatorType);

        var distance = await calculator.CalculateAsync(request, cancellationToken);

        _logger.LogInformation("Calculated distance: {Distance} km", distance);

        return (distance, calculatorType);
    }
}