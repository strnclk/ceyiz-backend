using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ceyiz.Application.DTOs;
using Ceyiz.Application.Features.Dashboard.Queries;

namespace Ceyiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetStats([FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetDashboardStatsQuery { UserId = userId });
        return Ok(result);
    }

    [HttpGet("budgets")]
    public async Task<ActionResult<List<BudgetDto>>> GetBudgets([FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetBudgetsQuery { UserId = userId });
        return Ok(result);
    }
}
