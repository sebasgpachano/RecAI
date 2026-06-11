using RecAI.Domain.Enums;

namespace RecAI.Domain.Entities;

public class Recommendation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public RecommendationStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    // Navigation property back to the owning user.
    public User User { get; set; } = null!;
}