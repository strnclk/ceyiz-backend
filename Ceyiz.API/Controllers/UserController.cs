using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ceyiz.Application.DTOs;
using Ceyiz.Application.Features.User.Commands;
using Ceyiz.Application.Features.User.Queries;

namespace Ceyiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("settings")]
    public async Task<ActionResult<UserSettingsDto>> GetSettings([FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetUserSettingsQuery { UserId = userId });
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }

    [HttpPut("settings")]
    public async Task<ActionResult<UserSettingsDto>> UpdateSettings([FromBody] UpdateUserSettingsDto settings, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new UpdateUserSettingsCommand { Settings = settings, UserId = userId });
        return Ok(result);
    }
}
