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

public class PartnerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PartnerInviteDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public PartnerDto Requester { get; set; } = null!;
    public PartnerDto Target { get; set; } = null!;
}
