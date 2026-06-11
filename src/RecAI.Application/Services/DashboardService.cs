using RecAI.Application.DTOs.Dashboard;
using RecAI.Application.Interfaces;
using RecAI.Domain.Enums;

namespace RecAI.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IRecommendationRepository _repository;

    public DashboardService(IRecommendationRepository repository)
        => _repository = repository;

    public async Task<DashboardResponse> GetStatsAsync(Guid userId, CancellationToken ct = default)
    {
        var counts = await _repository.GetStatusCountsForUserAsync(userId, ct);

        // GetValueOrDefault returns 0 if that status has no rows.
        return new DashboardResponse
        {
            Pending = counts.GetValueOrDefault(RecommendationStatus.Pending),
            Accepted = counts.GetValueOrDefault(RecommendationStatus.Accepted),
            Dismissed = counts.GetValueOrDefault(RecommendationStatus.Dismissed),
            Total = counts.Values.Sum()
        };
    }
}