using System.ComponentModel.DataAnnotations;

namespace DistanceCalculator.Dtos;

public class DistanceRequest
{
    [Required(ErrorMessage = "Point A is required")]
    public PointDto PointA { get; set; } = null!;

    [Required(ErrorMessage = "Point B is required")]
    public PointDto PointB { get; set; } = null!;
}
