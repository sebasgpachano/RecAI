using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecAI.Api.Extensions;
using RecAI.Application.DTOs.Recommendations;
using RecAI.Application.Interfaces;

namespace RecAI.Api.Controllers;

[ApiController]
[Authorize]                          // every endpoint requires a valid JWT
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationService _service;

    public RecommendationsController(IRecommendationService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<RecommendationResponse>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(User.GetUserId(), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecommendationResponse>> GetById(Guid id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, User.GetUserId(), ct));

    [HttpPost]
    public async Task<ActionResult<RecommendationResponse>> Create(
        [FromBody] CreateRecommendationRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(User.GetUserId(), request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RecommendationResponse>> Update(
        Guid id, [FromBody] UpdateRecommendationRequest request, CancellationToken ct)
        => Ok(await _service.UpdateAsync(id, User.GetUserId(), request, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, User.GetUserId(), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/accept")]
    public async Task<ActionResult<RecommendationResponse>> Accept(Guid id, CancellationToken ct)
        => Ok(await _service.AcceptAsync(id, User.GetUserId(), ct));

    [HttpPost("{id:guid}/dismiss")]
    public async Task<ActionResult<RecommendationResponse>> Dismiss(Guid id, CancellationToken ct)
        => Ok(await _service.DismissAsync(id, User.GetUserId(), ct));
}