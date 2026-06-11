using System.ComponentModel.DataAnnotations;
using RecAI.Domain.Enums;

namespace RecAI.Application.DTOs.Recommendations;

public class UpdateRecommendationRequest
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Priority Priority { get; set; }
}