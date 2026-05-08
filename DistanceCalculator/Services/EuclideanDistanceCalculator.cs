using DistanceCalculator.Dtos;

namespace DistanceCalculator.Services;

public class EuclideanDistanceCalculator : IDistanceCalculator
{
    public double Calculate(DistanceRequest request)
    {
        var latDiff = request.PointB.Latitude - request.PointA.Latitude;
        var lonDiff = request.PointB.Longitude - request.PointA.Longitude;
        return Math.Sqrt(latDiff * latDiff + lonDiff * lonDiff);
    }
}
