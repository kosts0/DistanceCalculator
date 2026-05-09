using DistanceCalculator.Dtos;

namespace DistanceCalculator.Services;

public interface IDistanceCalculator
{
    Task<double> CalculateAsync(DistanceRequest request, CancellationToken cancellationToken = default);
}
