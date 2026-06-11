using RecAI.Application.DTOs.Dashboard;

namespace RecAI.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponse> GetStatsAsync(Guid userId, CancellationToken ct = default);
}