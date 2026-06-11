using RecAI.Domain.Entities;

namespace RecAI.Application.Interfaces;

public interface IRecommendationRepository
{
    Task<List<Recommendation>> GetAllForUserAsync(Guid userId, CancellationToken ct = default);
    Task<Recommendation?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task AddAsync(Recommendation recommendation, CancellationToken ct = default);
    void Remove(Recommendation recommendation);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}