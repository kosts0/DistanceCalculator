using DistanceCalculator.Dtos;

namespace DistanceCalculator.Services;

public interface IDistanceService
{
    Task<DistanceResponse> CalculateDistanceAsync(DistanceRequest request);
}
