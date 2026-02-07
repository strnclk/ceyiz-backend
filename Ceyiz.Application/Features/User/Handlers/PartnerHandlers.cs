using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Ceyiz.Persistence.Context;
using Ceyiz.Application.Features.User.Commands;

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

        // Token'dan current user'ı al
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            Console.WriteLine("HttpContext is null - using fallback user");
            // Geçici çözüm: hardcoded user
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == "furkannsare@gmail.com", cancellationToken);
            
            if (currentUser == null)
            {
                Console.WriteLine("Fallback user not found");
                return false;
            }

            Console.WriteLine($"Linking {currentUser.Email} with {partner.Email}");

            currentUser.PartnerId = partner.Id;
            partner.PartnerId = currentUser.Id;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        var userIdClaim = httpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var currentUserId))
        {
            Console.WriteLine($"UserId claim not found or invalid: {userIdClaim?.Value}");
            return false;
        }

        var currentUserFromToken = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (currentUserFromToken == null)
        {
            Console.WriteLine($"Current user not found: {currentUserId}");
            return false;
        }

        Console.WriteLine($"Linking {currentUserFromToken.Email} with {partner.Email}");

        currentUserFromToken.PartnerId = partner.Id;
        partner.PartnerId = currentUserFromToken.Id;

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
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null || user.PartnerId == null)
            return false;

        var partner = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == user.PartnerId, cancellationToken);

        user.PartnerId = null;
        if (partner != null)
            partner.PartnerId = null;

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
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user?.PartnerId == null)
            return null;

        var partner = await _context.Users
            .Where(u => u.Id == user.PartnerId)
            .Select(u => new
            {
                id = u.Id,
                name = u.Name,
                email = u.Email,
                createdAt = u.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return partner;
    }
}
