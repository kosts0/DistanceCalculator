using DistanceCalculator.Dtos;
using DistanceCalculator.Enums;

namespace DistanceCalculator.Services;

public interface IDistanceService
{
    Task<(double Distance, CalculatorType CalculatorType)> CalculateDistanceAsync(
        DistanceRequest request,
        CalculatorType calculatorType = CalculatorType.Haversine,
        CancellationToken cancellationToken = default);
}
