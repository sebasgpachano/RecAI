using RecAI.Domain.Entities;
using RecAI.Domain.Enums;
using RecAI.Application.DTOs.Recommendations;

namespace RecAI.Application.Interfaces;

public interface IRecommendationRepository
{
    Task<(List<Recommendation> Items, string? NextCursor)> GetPageForUserAsync(
    Guid userId, RecommendationQueryParameters query, CancellationToken ct = default);
    Task<Recommendation?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task AddAsync(Recommendation recommendation, CancellationToken ct = default);
    void Remove(Recommendation recommendation);
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    Task<Dictionary<RecommendationStatus, int>> GetStatusCountsForUserAsync(
        Guid userId, CancellationToken ct = default);
}