namespace Ceyiz.Application.DTOs;

public class UserSettingsDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalBudget { get; set; }
    public string WeddingDate { get; set; } = string.Empty;
    public string PartnerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class UpdateUserSettingsDto
{
    public decimal TotalBudget { get; set; }
    public string WeddingDate { get; set; } = string.Empty;
    public string PartnerName { get; set; } = string.Empty;
}
