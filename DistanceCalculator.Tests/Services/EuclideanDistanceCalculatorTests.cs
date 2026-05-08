using DistanceCalculator.Dtos;
using DistanceCalculator.Services;

namespace DistanceCalculator.Tests.Services;

public class EuclideanDistanceCalculatorTests
{
    private readonly EuclideanDistanceCalculator _sut = new();

    [Test]
    public void Calculate_ThreeFourFiveTriangle_ReturnsFive()
    {
        var request = new DistanceRequest
        {
            PointA = new PointDto { Latitude = 0, Longitude = 0 },
            PointB = new PointDto { Latitude = 3, Longitude = 4 }
        };

        var result = _sut.Calculate(request);

        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void Calculate_SamePoint_ReturnsZero()
    {
        var request = new DistanceRequest
        {
            PointA = new PointDto { Latitude = 10, Longitude = 20 },
            PointB = new PointDto { Latitude = 10, Longitude = 20 }
        };

        var result = _sut.Calculate(request);

        Assert.That(result, Is.EqualTo(0));
    }
}
