using MediatR;
using Ceyiz.Application.DTOs;

namespace Ceyiz.Application.Features.User.Commands;

public class UpdateUserSettingsCommand : IRequest<UserSettingsDto>
{
    public UpdateUserSettingsDto Settings { get; set; } = null!;
    public Guid UserId { get; set; }
}
