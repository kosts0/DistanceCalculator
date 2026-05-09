using DistanceCalculator.Dtos;
using DistanceCalculator.Enums;
using DistanceCalculator.Exceptions;
using DistanceCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistanceCalculator.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class DistanceController : ControllerBase
{
    private readonly IDistanceService _distanceService;
    private readonly ILogger<DistanceController> _logger;

    public DistanceController(IDistanceService distanceService, ILogger<DistanceController> logger)
    {
        _distanceService = distanceService;
        _logger = logger;
    }

    /// <summary>
    /// Calculate the distance between two geographical points
    /// </summary>
    /// <param name="request">Coordinates of two points</param>
    /// <param name="calculatorType">Type of calculator to use (default: Haversine)</param>
    /// <returns>Distance in kilometers</returns>
    [HttpPost]
    public async Task<ActionResult<SuccessDistanceResponse>> CalculateDistance(
        [FromBody] DistanceRequest request,
        [FromQuery] CalculatorType calculatorType = CalculatorType.Haversine,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var (distance, calcType) = await _distanceService.CalculateDistanceAsync(request, calculatorType, cancellationToken);
            return Ok(new SuccessDistanceResponse(distance, calcType));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid calculator type requested: {CalculatorType}", calculatorType);
            return BadRequest(new ErrorResponse($"Invalid calculator type: {calculatorType}"));
        }
        catch (IntegrationException ex)
        {
            _logger.LogError(ex, "Integration error occurred: {Message}", ex.Message);
            return BadRequest(new ErrorResponse(ex.Message));
        }
    }
}