namespace Ceyiz.Application.DTOs;

public class DashboardStatsDto
{
    public int TotalItems { get; set; }
    public int CompletedItems { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal SpentBudget { get; set; }
    public int CompletionPercentage { get; set; }
    public decimal BudgetPercentage { get; set; }
    public int RemainingItems { get; set; }
    public decimal RemainingBudget { get; set; }
}

public class BudgetDto
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public int PercentageUsed { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBudgetDto
{
    public string Category { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

public class UpdateBudgetDto
{
    public string Category { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal SpentAmount { get; set; }
}
