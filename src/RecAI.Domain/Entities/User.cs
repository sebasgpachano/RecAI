namespace RecAI.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

    // Navigation property: a user has many recommendations.
    public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
}