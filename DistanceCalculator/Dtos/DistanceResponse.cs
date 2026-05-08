namespace DistanceCalculator.Dtos;

public class DistanceResponse
{
    public double DistanceInKm { get; init; }

    public DistanceResponse() { }

    public DistanceResponse(double distanceInKm)
    {
        DistanceInKm = Math.Round(distanceInKm, 3);
    }
}
