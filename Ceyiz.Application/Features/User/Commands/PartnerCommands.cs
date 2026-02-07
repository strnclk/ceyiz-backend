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

public class GetPartnerQuery : IRequest<object?>
{
    public Guid UserId { get; set; }
}
