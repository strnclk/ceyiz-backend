using MediatR;

namespace Ceyiz.Application.Features.User.Commands;

public class LinkPartnerCommand : IRequest<bool>
{
    public string PartnerEmail { get; set; } = string.Empty;
}

public class UnlinkPartnerCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
}

public class UnlinkPartnerByPartnerIdCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid PartnerId { get; set; }
}

public class GetPartnerQuery : IRequest<object?>
{
    public Guid UserId { get; set; }
}

public class GetPartnersQuery : IRequest<List<Ceyiz.Application.DTOs.PartnerDto>>
{
    public Guid UserId { get; set; }
}

public class GetIncomingPartnerInvitesQuery : IRequest<List<Ceyiz.Application.DTOs.PartnerInviteDto>>
{
    public Guid UserId { get; set; }
}

public class GetOutgoingPartnerInvitesQuery : IRequest<List<Ceyiz.Application.DTOs.PartnerInviteDto>>
{
    public Guid UserId { get; set; }
}

public class AcceptPartnerInviteCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid InvitationId { get; set; }
}

public class RejectPartnerInviteCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid InvitationId { get; set; }
}

public class CancelPartnerInviteCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid InvitationId { get; set; }
}
