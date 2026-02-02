using MediatR;
using Ceyiz.Application.DTOs;

namespace Ceyiz.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public LoginRequestDto LoginRequest { get; set; } = null!;
}

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public RegisterRequestDto RegisterRequest { get; set; } = null!;
}
