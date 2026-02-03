using MediatR;
using Microsoft.EntityFrameworkCore;
using Ceyiz.Persistence.Context;
using Ceyiz.Application.DTOs;
using Ceyiz.Application.Features.User.Commands;
using Ceyiz.Application.Features.User.Queries;
using Ceyiz.Domain.Entities;

namespace Ceyiz.Application.Features.User.Handlers;

public class GetUserSettingsQueryHandler : IRequestHandler<GetUserSettingsQuery, UserSettingsDto?>
{
    private readonly CeyizDbContext _context;

    public GetUserSettingsQueryHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<UserSettingsDto?> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _context.UserSettings
            .Where(s => s.UserId == request.UserId)
            .Select(s => new UserSettingsDto
            {
                Id = s.Id,
                UserId = s.UserId,
                TotalBudget = s.TotalBudget,
                WeddingDate = s.WeddingDate,
                PartnerName = s.PartnerName,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return settings;
    }
}

public class UpdateUserSettingsCommandHandler : IRequestHandler<UpdateUserSettingsCommand, UserSettingsDto>
{
    private readonly CeyizDbContext _context;

    public UpdateUserSettingsCommandHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<UserSettingsDto> Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.UserSettings
            .FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

        if (settings == null)
        {
            settings = new UserSettings
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                TotalBudget = request.Settings.TotalBudget,
                WeddingDate = request.Settings.WeddingDate,
                PartnerName = request.Settings.PartnerName,
                CreatedAt = DateTime.UtcNow
            };
            _context.UserSettings.Add(settings);
        }
        else
        {
            settings.TotalBudget = request.Settings.TotalBudget;
            settings.WeddingDate = request.Settings.WeddingDate;
            settings.PartnerName = request.Settings.PartnerName;
            settings.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UserSettingsDto
        {
            Id = settings.Id,
            UserId = settings.UserId,
            TotalBudget = settings.TotalBudget,
            WeddingDate = settings.WeddingDate,
            PartnerName = settings.PartnerName,
            CreatedAt = settings.CreatedAt,
            UpdatedAt = settings.UpdatedAt
        };
    }
}
