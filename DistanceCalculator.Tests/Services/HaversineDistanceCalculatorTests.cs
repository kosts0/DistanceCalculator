using DistanceCalculator.Dtos;
using DistanceCalculator.Services;

namespace DistanceCalculator.Tests.Services;

public class HaversineDistanceCalculatorTests
{
    private readonly HaversineDistanceCalculator _sut = new();

    [Test]
    public void Calculate_MoscowToSaintPetersburg_ReturnsAbout633Km()
    {
        var request = new DistanceRequest
        {
            PointA = new PointDto { Latitude = 55.7558, Longitude = 37.6173 },
            PointB = new PointDto { Latitude = 59.9343, Longitude = 30.3351 }
        };

        var result = _sut.Calculate(request);

        Assert.That(result, Is.EqualTo(633).Within(10));
    }

    [Test]
    public void Calculate_SamePoint_ReturnsZero()
    {
        var request = new DistanceRequest
        {
            PointA = new PointDto { Latitude = 55.7558, Longitude = 37.6173 },
            PointB = new PointDto { Latitude = 55.7558, Longitude = 37.6173 }
        };

        var result = _sut.Calculate(request);

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Calculate_NorthPoleToSouthPole_ReturnsHalfCircumference()
    {
        var request = new DistanceRequest
        {
            PointA = new PointDto { Latitude = 90, Longitude = 0 },
            PointB = new PointDto { Latitude = -90, Longitude = 0 }
        };

        var result = _sut.Calculate(request);
        var halfCircumference = Math.PI * 6371;

        Assert.That(result, Is.EqualTo(halfCircumference).Within(1));
    }
}
