using DistanceCalculator.Dtos;

namespace DistanceCalculator.Services;

public interface IDistanceCalculator
{
    double Calculate(DistanceRequest request);
}
