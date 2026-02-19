namespace Ceyiz.Domain.Entities;

public class UserPartnerLink
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PartnerUserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public User PartnerUser { get; set; } = null!;
}
