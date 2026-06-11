using RecAI.Application.DTOs.Recommendations;

namespace RecAI.Application.Interfaces;

public interface IRecommendationService
{
    Task<List<RecommendationResponse>> GetAllAsync(Guid userId, CancellationToken ct = default);
    Task<RecommendationResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<RecommendationResponse> CreateAsync(Guid userId, CreateRecommendationRequest request, CancellationToken ct = default);
    Task<RecommendationResponse> UpdateAsync(Guid id, Guid userId, UpdateRecommendationRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<RecommendationResponse> AcceptAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<RecommendationResponse> DismissAsync(Guid id, Guid userId, CancellationToken ct = default);
}