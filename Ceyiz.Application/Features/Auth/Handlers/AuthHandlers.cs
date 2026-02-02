using MediatR;
using Ceyiz.Application.DTOs;
using Ceyiz.Application.Services;
using Ceyiz.Application.Features.Auth.Commands;

namespace Ceyiz.Application.Features.Auth.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IAuthService _authService;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IAuthService authService, IJwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var isValid = await _authService.VerifyPasswordAsync(request.LoginRequest.Email, request.LoginRequest.Password);
        
        if (!isValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var user = await _authService.GetUserByEmailAsync(request.LoginRequest.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var token = _jwtService.GenerateToken(user.Id, user.Email);

        return new AuthResponseDto
        {
            Token = token,
            User = user
        };
    }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IAuthService _authService;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IAuthService authService, IJwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _authService.GetUserByEmailAsync(request.RegisterRequest.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var user = await _authService.CreateUserAsync(request.RegisterRequest);
        var token = _jwtService.GenerateToken(user.Id, user.Email);

        return new AuthResponseDto
        {
            Token = token,
            User = user
        };
    }
}
