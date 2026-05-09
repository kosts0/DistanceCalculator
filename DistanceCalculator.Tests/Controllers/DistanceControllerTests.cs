using DistanceCalculator.Controllers;
using DistanceCalculator.Dtos;
using DistanceCalculator.Enums;
using DistanceCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Tests.Controllers;

public class DistanceControllerTests
{
    [Test]
    public async Task CalculateDistance_ReturnsOkObjectResult()
    {
        var mockService = new Mock<IDistanceService>();
        mockService
            .Setup(s => s.CalculateDistanceAsync(
                It.IsAny<DistanceRequest>(),
                It.IsAny<CalculatorType>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((500.0, CalculatorType.Haversine));

        var mockLogger = new Mock<ILogger<DistanceController>>();
        var sut = new DistanceController(mockService.Object, mockLogger.Object);

        var request = new DistanceRequest
        {
            PointA = new PointDto { Latitude = 55.7558, Longitude = 37.6173 },
            PointB = new PointDto { Latitude = 59.9343, Longitude = 30.3351 }
        };

        var result = await sut.CalculateDistance(request, CalculatorType.Haversine, CancellationToken.None);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult!.Value, Is.TypeOf<SuccessDistanceResponse>());
        var response = okResult.Value as SuccessDistanceResponse;
        Assert.That(response!.Status, Is.EqualTo("SUCCESS"));
        Assert.That(response.Data.DistanceInKm, Is.EqualTo(500.0));
        Assert.That(response.Data.CalculatorType, Is.EqualTo(CalculatorType.Haversine));
    }
}