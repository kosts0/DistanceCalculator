using DistanceCalculator.Dtos;

namespace DistanceCalculator.Services;

public class HaversineDistanceCalculator : IDistanceCalculator
{
    private const double EarthRadiusKm = 6371.0;

    public Task<double> CalculateAsync(DistanceRequest request, CancellationToken cancellationToken = default)
    {
        var latDistance = ToRadians(request.PointB.Latitude - request.PointA.Latitude);
        var lonDistance = ToRadians(request.PointB.Longitude - request.PointA.Longitude);

        var latA = ToRadians(request.PointA.Latitude);
        var latB = ToRadians(request.PointB.Latitude);

        var a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2) +
                Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2) *
                Math.Cos(latA) * Math.Cos(latB);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return Task.FromResult(EarthRadiusKm * c);
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}
