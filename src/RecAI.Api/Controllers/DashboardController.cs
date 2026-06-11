using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecAI.Api.Extensions;
using RecAI.Application.DTOs.Dashboard;
using RecAI.Application.Interfaces;

namespace RecAI.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<DashboardResponse>> Get(CancellationToken ct)
        => Ok(await _service.GetStatsAsync(User.GetUserId(), ct));
}