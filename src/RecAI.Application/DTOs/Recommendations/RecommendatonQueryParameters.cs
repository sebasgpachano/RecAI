using RecAI.Domain.Enums;

namespace RecAI.Application.DTOs.Recommendations;

public class RecommendationQueryParameters
{
    public RecommendationStatus? Status { get; set; }   // null = no filter
    public Priority? Priority { get; set; }
    public string? Search { get; set; }
    public int? PageSize { get; set; }   // null → default
    public string? Cursor { get; set; }  // null → first page
}