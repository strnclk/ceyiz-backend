using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Ceyiz.Persistence.Context;
using Ceyiz.Application.Features.User.Commands;
using Ceyiz.Application.DTOs;
using Ceyiz.Domain.Entities;

namespace Ceyiz.Application.Features.User.Handlers;

public class LinkPartnerCommandHandler : IRequestHandler<LinkPartnerCommand, bool>
{
    private readonly CeyizDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LinkPartnerCommandHandler(CeyizDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(LinkPartnerCommand request, CancellationToken cancellationToken)
    {
        var partner = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.PartnerEmail, cancellationToken);

        if (partner == null)
        {
            Console.WriteLine($"Partner not found: {request.PartnerEmail}");
            return false;
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return false;

        var userIdClaim = httpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var currentUserId))
            return false;

        if (currentUserId == partner.Id)
            return false;

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (currentUser == null)
            return false;

        var alreadyLinked = await _context.UserPartnerLinks
            .AnyAsync(l => l.UserId == currentUserId && l.PartnerUserId == partner.Id, cancellationToken);

        if (alreadyLinked)
            return true;

        var existingInvite = await _context.PartnerInvitations
            .FirstOrDefaultAsync(i => i.RequesterUserId == currentUserId && i.TargetUserId == partner.Id, cancellationToken);

        if (existingInvite != null)
        {
            if (existingInvite.Status == PartnerInvitationStatus.Pending)
                return true;

            existingInvite.Status = PartnerInvitationStatus.Pending;
            existingInvite.CreatedAt = DateTime.UtcNow;
            existingInvite.RespondedAt = null;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        _context.PartnerInvitations.Add(new PartnerInvitation
        {
            Id = Guid.NewGuid(),
            RequesterUserId = currentUserId,
            TargetUserId = partner.Id,
            Status = PartnerInvitationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class UnlinkPartnerCommandHandler : IRequestHandler<UnlinkPartnerCommand, bool>
{
    private readonly CeyizDbContext _context;

    public UnlinkPartnerCommandHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UnlinkPartnerCommand request, CancellationToken cancellationToken)
    {
        var links = await _context.UserPartnerLinks
            .Where(l => l.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        if (links.Count == 0)
            return false;

        var partnerIds = links.Select(l => l.PartnerUserId).Distinct().ToList();

        _context.UserPartnerLinks.RemoveRange(links);

        var reverseLinks = await _context.UserPartnerLinks
            .Where(l => partnerIds.Contains(l.UserId) && l.PartnerUserId == request.UserId)
            .ToListAsync(cancellationToken);

        _context.UserPartnerLinks.RemoveRange(reverseLinks);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user != null)
            user.PartnerId = null;

        var legacyPartners = await _context.Users.Where(u => u.PartnerId == request.UserId).ToListAsync(cancellationToken);
        foreach (var lp in legacyPartners)
            lp.PartnerId = null;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class UnlinkPartnerByPartnerIdCommandHandler : IRequestHandler<UnlinkPartnerByPartnerIdCommand, bool>
{
    private readonly CeyizDbContext _context;

    public UnlinkPartnerByPartnerIdCommandHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UnlinkPartnerByPartnerIdCommand request, CancellationToken cancellationToken)
    {
        var links = await _context.UserPartnerLinks
            .Where(l => l.UserId == request.UserId && l.PartnerUserId == request.PartnerId)
            .ToListAsync(cancellationToken);

        var reverseLinks = await _context.UserPartnerLinks
            .Where(l => l.UserId == request.PartnerId && l.PartnerUserId == request.UserId)
            .ToListAsync(cancellationToken);

        if (links.Count == 0 && reverseLinks.Count == 0)
            return false;

        _context.UserPartnerLinks.RemoveRange(links);
        _context.UserPartnerLinks.RemoveRange(reverseLinks);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class GetPartnerQueryHandler : IRequestHandler<GetPartnerQuery, object?>
{
    private readonly CeyizDbContext _context;

    public GetPartnerQueryHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<object?> Handle(GetPartnerQuery request, CancellationToken cancellationToken)
    {
        var partnerId = await _context.UserPartnerLinks
            .Where(l => l.UserId == request.UserId)
            .Select(l => l.PartnerUserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (partnerId == Guid.Empty)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            partnerId = user?.PartnerId ?? Guid.Empty;
        }

        if (partnerId == Guid.Empty)
            return null;

        var partner = await _context.Users
            .Where(u => u.Id == partnerId)
            .Select(u => new PartnerDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return partner;
    }
}

public class GetPartnersQueryHandler : IRequestHandler<GetPartnersQuery, List<PartnerDto>>
{
    private readonly CeyizDbContext _context;

    public GetPartnersQueryHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<List<PartnerDto>> Handle(GetPartnersQuery request, CancellationToken cancellationToken)
    {
        var partnerIds = await _context.UserPartnerLinks
            .Where(l => l.UserId == request.UserId)
            .Select(l => l.PartnerUserId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var partners = await _context.Users
            .Where(u => partnerIds.Contains(u.Id))
            .Select(u => new PartnerDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return partners;
    }
}

public class GetIncomingPartnerInvitesQueryHandler : IRequestHandler<GetIncomingPartnerInvitesQuery, List<PartnerInviteDto>>
{
    private readonly CeyizDbContext _context;

    public GetIncomingPartnerInvitesQueryHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<List<PartnerInviteDto>> Handle(GetIncomingPartnerInvitesQuery request, CancellationToken cancellationToken)
    {
        var invites = await _context.PartnerInvitations
            .Where(i => i.TargetUserId == request.UserId && i.Status == PartnerInvitationStatus.Pending)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new PartnerInviteDto
            {
                Id = i.Id,
                Status = i.Status.ToString(),
                CreatedAt = i.CreatedAt,
                Requester = new PartnerDto
                {
                    Id = i.RequesterUser.Id,
                    Name = i.RequesterUser.Name,
                    Email = i.RequesterUser.Email,
                    CreatedAt = i.RequesterUser.CreatedAt
                },
                Target = new PartnerDto
                {
                    Id = i.TargetUser.Id,
                    Name = i.TargetUser.Name,
                    Email = i.TargetUser.Email,
                    CreatedAt = i.TargetUser.CreatedAt
                }
            })
            .ToListAsync(cancellationToken);

        return invites;
    }
}

public class GetOutgoingPartnerInvitesQueryHandler : IRequestHandler<GetOutgoingPartnerInvitesQuery, List<PartnerInviteDto>>
{
    private readonly CeyizDbContext _context;

    public GetOutgoingPartnerInvitesQueryHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<List<PartnerInviteDto>> Handle(GetOutgoingPartnerInvitesQuery request, CancellationToken cancellationToken)
    {
        var invites = await _context.PartnerInvitations
            .Where(i => i.RequesterUserId == request.UserId && i.Status == PartnerInvitationStatus.Pending)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new PartnerInviteDto
            {
                Id = i.Id,
                Status = i.Status.ToString(),
                CreatedAt = i.CreatedAt,
                Requester = new PartnerDto
                {
                    Id = i.RequesterUser.Id,
                    Name = i.RequesterUser.Name,
                    Email = i.RequesterUser.Email,
                    CreatedAt = i.RequesterUser.CreatedAt
                },
                Target = new PartnerDto
                {
                    Id = i.TargetUser.Id,
                    Name = i.TargetUser.Name,
                    Email = i.TargetUser.Email,
                    CreatedAt = i.TargetUser.CreatedAt
                }
            })
            .ToListAsync(cancellationToken);

        return invites;
    }
}

public class AcceptPartnerInviteCommandHandler : IRequestHandler<AcceptPartnerInviteCommand, bool>
{
    private readonly CeyizDbContext _context;

    public AcceptPartnerInviteCommandHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AcceptPartnerInviteCommand request, CancellationToken cancellationToken)
    {
        var invite = await _context.PartnerInvitations
            .FirstOrDefaultAsync(i => i.Id == request.InvitationId && i.TargetUserId == request.UserId, cancellationToken);

        if (invite == null || invite.Status != PartnerInvitationStatus.Pending)
            return false;

        var alreadyLinked = await _context.UserPartnerLinks
            .AnyAsync(l => l.UserId == invite.RequesterUserId && l.PartnerUserId == invite.TargetUserId, cancellationToken);

        if (!alreadyLinked)
        {
            _context.UserPartnerLinks.Add(new UserPartnerLink
            {
                Id = Guid.NewGuid(),
                UserId = invite.RequesterUserId,
                PartnerUserId = invite.TargetUserId,
                CreatedAt = DateTime.UtcNow
            });

            _context.UserPartnerLinks.Add(new UserPartnerLink
            {
                Id = Guid.NewGuid(),
                UserId = invite.TargetUserId,
                PartnerUserId = invite.RequesterUserId,
                CreatedAt = DateTime.UtcNow
            });
        }

        invite.Status = PartnerInvitationStatus.Accepted;
        invite.RespondedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class RejectPartnerInviteCommandHandler : IRequestHandler<RejectPartnerInviteCommand, bool>
{
    private readonly CeyizDbContext _context;

    public RejectPartnerInviteCommandHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RejectPartnerInviteCommand request, CancellationToken cancellationToken)
    {
        var invite = await _context.PartnerInvitations
            .FirstOrDefaultAsync(i => i.Id == request.InvitationId && i.TargetUserId == request.UserId, cancellationToken);

        if (invite == null || invite.Status != PartnerInvitationStatus.Pending)
            return false;

        invite.Status = PartnerInvitationStatus.Rejected;
        invite.RespondedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class CancelPartnerInviteCommandHandler : IRequestHandler<CancelPartnerInviteCommand, bool>
{
    private readonly CeyizDbContext _context;

    public CancelPartnerInviteCommandHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CancelPartnerInviteCommand request, CancellationToken cancellationToken)
    {
        var invite = await _context.PartnerInvitations
            .FirstOrDefaultAsync(i => i.Id == request.InvitationId && i.RequesterUserId == request.UserId, cancellationToken);

        if (invite == null || invite.Status != PartnerInvitationStatus.Pending)
            return false;

        invite.Status = PartnerInvitationStatus.Canceled;
        invite.RespondedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
