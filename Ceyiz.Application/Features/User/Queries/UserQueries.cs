using MediatR;

namespace Ceyiz.Application.Features.User.Queries;

public class GetUserSettingsQuery : IRequest<Ceyiz.Application.DTOs.UserSettingsDto?>
{
    public Guid UserId { get; set; }
}
