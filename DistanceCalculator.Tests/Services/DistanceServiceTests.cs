using DistanceCalculator.Dtos;
using DistanceCalculator.Services;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Tests.Services;

public class DistanceServiceTests
{
    [Test]
    public async Task CalculateDistanceAsync_ReturnsCorrectResponse()
    {
        var mockCalculator = new Mock<IDistanceCalculator>();
        mockCalculator.Setup(c => c.Calculate(It.IsAny<DistanceRequest>())).Returns(100.0);

        var mockLogger = new Mock<ILogger<DistanceService>>();
        var sut = new DistanceService(mockCalculator.Object, mockLogger.Object);

        var request = new DistanceRequest
        {
            PointA = new PointDto { Latitude = 55.7558, Longitude = 37.6173 },
            PointB = new PointDto { Latitude = 59.9343, Longitude = 30.3351 }
        };

        var result = await sut.CalculateDistanceAsync(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.DistanceInKm, Is.EqualTo(100.0));
    }
}
