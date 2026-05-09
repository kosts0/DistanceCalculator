using DistanceCalculator.Dtos;
using DistanceCalculator.Enums;
using DistanceCalculator.Services;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Tests.Services;

public class DistanceServiceTests
{
    [Test]
    public async Task CalculateDistanceAsync_ReturnsCorrectResponse()
    {
        var mockCalculator = new Mock<IDistanceCalculator>();
        mockCalculator.Setup(c => c.CalculateAsync(It.IsAny<DistanceRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(100.0);

        var mockFactory = new Mock<ICalculatorFactory>();
        mockFactory.Setup(f => f.GetCalculator(CalculatorType.Haversine)).Returns(mockCalculator.Object);

        var mockLogger = new Mock<ILogger<DistanceService>>();
        var sut = new DistanceService(mockFactory.Object, mockLogger.Object);

        var request = new DistanceRequest
        {
            PointA = new PointDto { Latitude = 55.7558, Longitude = 37.6173 },
            PointB = new PointDto { Latitude = 59.9343, Longitude = 30.3351 }
        };

        var (distance, calcType) = await sut.CalculateDistanceAsync(request);

        Assert.That(distance, Is.EqualTo(100.0));
        Assert.That(calcType, Is.EqualTo(CalculatorType.Haversine));
    }
}