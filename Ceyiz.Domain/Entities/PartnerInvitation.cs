namespace Ceyiz.Domain.Entities;

public enum PartnerInvitationStatus
{
    Pending = 0,
    Accepted = 1,
    Rejected = 2,
    Canceled = 3
}

public class PartnerInvitation
{
    public Guid Id { get; set; }
    public Guid RequesterUserId { get; set; }
    public Guid TargetUserId { get; set; }
    public PartnerInvitationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RespondedAt { get; set; }

    public User RequesterUser { get; set; } = null!;
    public User TargetUser { get; set; } = null!;
}
