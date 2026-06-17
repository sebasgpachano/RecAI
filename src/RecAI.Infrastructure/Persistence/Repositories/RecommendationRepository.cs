using Microsoft.EntityFrameworkCore;
using RecAI.Application.Interfaces;
using RecAI.Domain.Entities;
using RecAI.Domain.Enums;
using RecAI.Application.DTOs.Recommendations;
using System.Text;

namespace RecAI.Infrastructure.Persistence.Repositories;

public class RecommendationRepository : IRecommendationRepository
{
    private readonly AppDbContext _context;

    public RecommendationRepository(AppDbContext context) => _context = context;

    public async Task<(List<Recommendation> Items, string? NextCursor)> GetPageForUserAsync(
    Guid userId, RecommendationQueryParameters query, CancellationToken ct = default)
    {
        var pageSize = Math.Clamp(query.PageSize ?? 20, 1, 100);

        IQueryable<Recommendation> q = _context.Recommendations
            .AsNoTracking()
            .Where(r => r.UserId == userId);

        if (query.Status is not null) q = q.Where(r => r.Status == query.Status);
        if (query.Priority is not null) q = q.Where(r => r.Priority == query.Priority);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search.Trim()}%";
            q = q.Where(r => EF.Functions.ILike(r.Title, pattern) || EF.Functions.ILike(r.Description, pattern));
        }

        // Keyset: only rows older than the cursor (the last item the client saw).
        if (TryDecodeCursor(query.Cursor, out var cursorCreatedAt))
            q = q.Where(r => r.CreatedAt < cursorCreatedAt);

        // Fetch one extra row to know whether there's a next page.
        var rows = await q
            .OrderByDescending(r => r.CreatedAt)
            .Take(pageSize + 1)
            .ToListAsync(ct);

        string? nextCursor = null;
        if (rows.Count > pageSize)
        {
            rows.RemoveAt(rows.Count - 1);                  // drop the probe row
            nextCursor = EncodeCursor(rows[^1].CreatedAt);  // cursor = last item returned
        }

        return (rows, nextCursor);
    }

    private static string EncodeCursor(DateTimeOffset createdAt) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(createdAt.ToString("O")));

    private static bool TryDecodeCursor(string? cursor, out DateTimeOffset createdAt)
    {
        createdAt = default;
        if (string.IsNullOrWhiteSpace(cursor)) return false;
        try
        {
            var raw = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
            createdAt = DateTimeOffset.Parse(raw, null, System.Globalization.DateTimeStyles.RoundtripKind);
            return true;
        }
        catch { return false; }
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