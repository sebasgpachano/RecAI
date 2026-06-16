using Microsoft.EntityFrameworkCore;
using RecAI.Application.Interfaces;
using RecAI.Domain.Entities;
using RecAI.Domain.Enums;
using RecAI.Application.DTOs.Recommendations;

namespace RecAI.Infrastructure.Persistence.Repositories;

public class RecommendationRepository : IRecommendationRepository
{
    private readonly AppDbContext _context;

    public RecommendationRepository(AppDbContext context) => _context = context;

    public async Task<List<Recommendation>> GetAllForUserAsync(
    Guid userId, RecommendationQueryParameters query, CancellationToken ct = default)
    {
        // Base query — deferred: nothing hits the database yet.
        IQueryable<Recommendation> q = _context.Recommendations
            .AsNoTracking()
            .Where(r => r.UserId == userId);

        if (query.Status is not null)
            q = q.Where(r => r.Status == query.Status);

        if (query.Priority is not null)
            q = q.Where(r => r.Priority == query.Priority);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search.Trim()}%";
            // ILike → PostgreSQL case-insensitive ILIKE, runs in the database.
            q = q.Where(r =>
                EF.Functions.ILike(r.Title, pattern) ||
                EF.Functions.ILike(r.Description, pattern));
        }

        return await q
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);   // ← single SQL query built and executed here
    }

    public async Task<Recommendation?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken ct = default) =>
        await _context.Recommendations
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId, ct);

    public async Task AddAsync(Recommendation recommendation, CancellationToken ct = default) =>
        await _context.Recommendations.AddAsync(recommendation, ct);

    public void Remove(Recommendation recommendation) =>
        _context.Recommendations.Remove(recommendation);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);

    public async Task<Dictionary<RecommendationStatus, int>> GetStatusCountsForUserAsync(
    Guid userId, CancellationToken ct = default) =>
    await _context.Recommendations
        .Where(r => r.UserId == userId)
        .GroupBy(r => r.Status)
        .Select(g => new { Status = g.Key, Count = g.Count() })
        .ToDictionaryAsync(x => x.Status, x => x.Count, ct);
}