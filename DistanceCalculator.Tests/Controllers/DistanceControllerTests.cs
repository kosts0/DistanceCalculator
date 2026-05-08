using DistanceCalculator.Controllers;
using DistanceCalculator.Dtos;
using DistanceCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistanceCalculator.Tests.Controllers;

public class DistanceControllerTests
{
    [Test]
    public async Task CalculateDistance_ReturnsOkObjectResult()
    {
        var expectedResponse = new DistanceResponse(500.0);
        var mockService = new Mock<IDistanceService>();
        mockService
            .Setup(s => s.CalculateDistanceAsync(It.IsAny<DistanceRequest>()))
            .ReturnsAsync(expectedResponse);

        var sut = new DistanceController(mockService.Object);

        var request = new DistanceRequest
        {
            PointA = new PointDto { Latitude = 55.7558, Longitude = 37.6173 },
            PointB = new PointDto { Latitude = 59.9343, Longitude = 30.3351 }
        };

        var result = await sut.CalculateDistance(request);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult!.Value, Is.TypeOf<DistanceResponse>());
        var response = okResult.Value as DistanceResponse;
        Assert.That(response!.DistanceInKm, Is.EqualTo(500.0));
    }
}
