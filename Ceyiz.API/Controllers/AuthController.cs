using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ceyiz.Application.DTOs;
using Ceyiz.Application.Features.Auth.Commands;

namespace Ceyiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        var result = await _mediator.Send(new LoginCommand { LoginRequest = request });
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _mediator.Send(new RegisterCommand { RegisterRequest = request });
        return Ok(result);
    }
}
