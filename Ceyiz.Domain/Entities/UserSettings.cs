namespace Ceyiz.Domain.Entities;

public class UserSettings
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalBudget { get; set; }
    public string WeddingDate { get; set; } = string.Empty;
    public string PartnerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation property
    public User User { get; set; } = null!;
}
