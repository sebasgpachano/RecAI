using Microsoft.Extensions.Logging;
using RecAI.Application.DTOs.Recommendations;
using RecAI.Application.Exceptions;
using RecAI.Application.Interfaces;
using RecAI.Domain.Entities;
using RecAI.Domain.Enums;

namespace RecAI.Application.Services;

public class RecommendationService : IRecommendationService
{
    private readonly IRecommendationRepository _repository;
    private readonly ILogger<RecommendationService> _logger;

    public RecommendationService(
        IRecommendationRepository repository,
        ILogger<RecommendationService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<RecommendationResponse>> GetAllAsync(Guid userId, CancellationToken ct = default)
    {
        var items = await _repository.GetAllForUserAsync(userId, ct);
        return items.Select(MapToResponse).ToList();
    }

    public async Task<RecommendationResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var item = await _repository.GetByIdForUserAsync(id, userId, ct)
                   ?? throw new NotFoundException($"Recommendation {id} not found.");
        return MapToResponse(item);
    }

    public async Task<RecommendationResponse> CreateAsync(Guid userId, CreateRecommendationRequest request, CancellationToken ct = default)
    {
        var recommendation = new Recommendation
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = request.Priority,
            Status = RecommendationStatus.Pending,   // always starts Pending
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _repository.AddAsync(recommendation, ct);
        await _repository.SaveChangesAsync(ct);

        _logger.LogInformation("User {UserId} created recommendation {Id}", userId, recommendation.Id);
        return MapToResponse(recommendation);
    }

    public async Task<RecommendationResponse> UpdateAsync(Guid id, Guid userId, UpdateRecommendationRequest request, CancellationToken ct = default)
    {
        var item = await _repository.GetByIdForUserAsync(id, userId, ct)
                   ?? throw new NotFoundException($"Recommendation {id} not found.");

        item.Title = request.Title.Trim();
        item.Description = request.Description.Trim();
        item.Priority = request.Priority;
        item.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.SaveChangesAsync(ct);   // EF tracks the changes automatically
        return MapToResponse(item);
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var item = await _repository.GetByIdForUserAsync(id, userId, ct)
                   ?? throw new NotFoundException($"Recommendation {id} not found.");

        _repository.Remove(item);
        await _repository.SaveChangesAsync(ct);
        _logger.LogInformation("User {UserId} deleted recommendation {Id}", userId, id);
    }

    public Task<RecommendationResponse> AcceptAsync(Guid id, Guid userId, CancellationToken ct = default) =>
        ChangeStatusAsync(id, userId, RecommendationStatus.Accepted, ct);

    public Task<RecommendationResponse> DismissAsync(Guid id, Guid userId, CancellationToken ct = default) =>
        ChangeStatusAsync(id, userId, RecommendationStatus.Dismissed, ct);

    private async Task<RecommendationResponse> ChangeStatusAsync(
        Guid id, Guid userId, RecommendationStatus status, CancellationToken ct)
    {
        var item = await _repository.GetByIdForUserAsync(id, userId, ct)
                   ?? throw new NotFoundException($"Recommendation {id} not found.");

        item.Status = status;
        item.UpdatedAt = DateTimeOffset.UtcNow;
        await _repository.SaveChangesAsync(ct);
        return MapToResponse(item);
    }

    private static RecommendationResponse MapToResponse(Recommendation r) => new()
    {
        Id = r.Id,
        Title = r.Title,
        Description = r.Description,
        Priority = r.Priority,
        Status = r.Status,
        CreatedAt = r.CreatedAt,
        UpdatedAt = r.UpdatedAt
    };
}