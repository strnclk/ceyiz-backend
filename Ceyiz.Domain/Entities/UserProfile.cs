namespace Ceyiz.Domain.Entities;

public class UserProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? WeddingDate { get; set; }
    public string? PartnerName { get; set; }
    public string? WeddingVenue { get; set; }
    public int? GuestCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation property
    public User User { get; set; } = null!;
}
