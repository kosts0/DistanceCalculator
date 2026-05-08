using DistanceCalculator.Dtos;
using DistanceCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistanceCalculator.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class DistanceController : ControllerBase
{
    private readonly IDistanceService _distanceService;

    public DistanceController(IDistanceService distanceService)
    {
        _distanceService = distanceService;
    }

    /// <summary>
    /// Calculate the distance between two geographical points
    /// </summary>
    /// <param name="request">Coordinates of two points</param>
    /// <returns>Distance in kilometers</returns>
    /// <response code="200">Distance calculated successfully</response>
    /// <response code="400">Invalid request parameters</response>
    /// <response code="408">Request timed out</response>
    [HttpPost]
    [ProducesResponseType(typeof(DistanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
    public async Task<ActionResult<DistanceResponse>> CalculateDistance([FromBody] DistanceRequest request)
    {
        var response = await _distanceService.CalculateDistanceAsync(request);
        return Ok(response);
    }
}
