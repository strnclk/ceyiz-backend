using MediatR;
using Microsoft.EntityFrameworkCore;
using Ceyiz.Persistence.Context;
using Ceyiz.Application.DTOs;
using Ceyiz.Application.Features.Dashboard.Queries;

namespace Ceyiz.Application.Features.Dashboard.Handlers;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly CeyizDbContext _context;

    public GetDashboardStatsQueryHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        // Toplam çeyiz ürünleri
        var totalItems = await _context.TrousseauItems
            .Where(i => i.UserId == userId)
            .CountAsync(cancellationToken);

        // Tamamlanan ürünler
        var completedItems = await _context.TrousseauItems
            .Where(i => i.UserId == userId && i.IsCompleted)
            .CountAsync(cancellationToken);

        // Toplam bütçe (trousseau ürünlerinden)
        var totalBudget = await _context.TrousseauItems
            .Where(i => i.UserId == userId)
            .SumAsync(i => i.Price, cancellationToken);

        // Harcanan bütçe (tamamlanan ürünlerden)
        var spentBudget = await _context.TrousseauItems
            .Where(i => i.UserId == userId && i.IsCompleted)
            .SumAsync(i => i.Price, cancellationToken);

        // Hesaplamalar
        var completionPercentage = totalItems > 0 ? (double)completedItems / totalItems * 100 : 0;
        var remainingItems = totalItems - completedItems;
        var remainingBudget = totalBudget - spentBudget;
        var budgetPercentage = totalBudget > 0 ? (double)spentBudget / (double)totalBudget * 100 : 0;

        return new DashboardStatsDto
        {
            TotalItems = totalItems,
            CompletedItems = completedItems,
            RemainingItems = remainingItems,
            TotalBudget = totalBudget,
            SpentBudget = spentBudget,
            RemainingBudget = remainingBudget,
            CompletionPercentage = (int)completionPercentage,
            BudgetPercentage = (decimal)budgetPercentage
        };
    }
}

public class GetBudgetsQueryHandler : IRequestHandler<GetBudgetsQuery, List<BudgetDto>>
{
    private readonly CeyizDbContext _context;

    public GetBudgetsQueryHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<List<BudgetDto>> Handle(GetBudgetsQuery request, CancellationToken cancellationToken)
    {
        var budgets = await _context.Budgets
            .Where(b => b.UserId == request.UserId)
            .Select(b => new BudgetDto
            {
                Id = b.Id,
                Category = b.Category,
                TotalAmount = b.TotalAmount,
                SpentAmount = b.SpentAmount,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return budgets;
    }
}
