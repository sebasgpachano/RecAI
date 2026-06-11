using Microsoft.EntityFrameworkCore;
using RecAI.Application.Interfaces;
using RecAI.Domain.Entities;

namespace RecAI.Infrastructure.Persistence.Repositories;

public class RecommendationRepository : IRecommendationRepository
{
    private readonly AppDbContext _context;

    public RecommendationRepository(AppDbContext context) => _context = context;

    public async Task<List<Recommendation>> GetAllForUserAsync(Guid userId, CancellationToken ct = default) =>
        await _context.Recommendations
            .AsNoTracking()                       // read-only: no need to track changes
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

    public async Task<Recommendation?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken ct = default) =>
        await _context.Recommendations
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId, ct);

    public async Task AddAsync(Recommendation recommendation, CancellationToken ct = default) =>
        await _context.Recommendations.AddAsync(recommendation, ct);

    public void Remove(Recommendation recommendation) =>
        _context.Recommendations.Remove(recommendation);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}