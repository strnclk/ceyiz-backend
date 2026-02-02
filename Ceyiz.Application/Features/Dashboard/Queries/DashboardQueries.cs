using MediatR;
using Ceyiz.Application.DTOs;

namespace Ceyiz.Application.Features.Dashboard.Queries;

public class GetDashboardStatsQuery : IRequest<DashboardStatsDto>
{
    public Guid UserId { get; set; }
}

public class GetBudgetsQuery : IRequest<List<BudgetDto>>
{
    public Guid UserId { get; set; }
}
