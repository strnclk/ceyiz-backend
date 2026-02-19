using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Ceyiz.Application.DTOs;
using Ceyiz.Application.Features.User.Commands;
using Ceyiz.Application.Features.User.Queries;

namespace Ceyiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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

    [HttpPost("partner")]
    [Authorize]
    public async Task<ActionResult> LinkPartner([FromBody] LinkPartnerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("partner")]
    [Authorize]
    public async Task<ActionResult> UnlinkPartner()
    {
        var userId = GetUserIdFromToken();
        var result = await _mediator.Send(new UnlinkPartnerCommand { UserId = userId });
        return Ok(result);
    }

    [HttpGet("partner")]
    [Authorize]
    public async Task<ActionResult> GetPartner()
    {
        var userId = GetUserIdFromToken();
        var result = await _mediator.Send(new GetPartnerQuery { UserId = userId });
        return Ok(result);
    }

    [HttpGet("partners")]
    [Authorize]
    public async Task<ActionResult> GetPartners()
    {
        var userId = GetUserIdFromToken();
        var result = await _mediator.Send(new GetPartnersQuery { UserId = userId });
        return Ok(result);
    }

    [HttpPost("partners")]
    [Authorize]
    public async Task<ActionResult> LinkPartnerMulti([FromBody] LinkPartnerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("partners/{partnerId}")]
    [Authorize]
    public async Task<ActionResult> UnlinkPartnerMulti(Guid partnerId)
    {
        var userId = GetUserIdFromToken();
        var result = await _mediator.Send(new UnlinkPartnerByPartnerIdCommand { UserId = userId, PartnerId = partnerId });
        return Ok(result);
    }

    [HttpGet("partner-invites/incoming")]
    [Authorize]
    public async Task<ActionResult> GetIncomingPartnerInvites()
    {
        var userId = GetUserIdFromToken();
        var result = await _mediator.Send(new GetIncomingPartnerInvitesQuery { UserId = userId });
        return Ok(result);
    }

    [HttpGet("partner-invites/outgoing")]
    [Authorize]
    public async Task<ActionResult> GetOutgoingPartnerInvites()
    {
        var userId = GetUserIdFromToken();
        var result = await _mediator.Send(new GetOutgoingPartnerInvitesQuery { UserId = userId });
        return Ok(result);
    }

    [HttpPost("partner-invites/{invitationId}/accept")]
    [Authorize]
    public async Task<ActionResult> AcceptPartnerInvite(Guid invitationId)
    {
        var userId = GetUserIdFromToken();
        var result = await _mediator.Send(new AcceptPartnerInviteCommand { UserId = userId, InvitationId = invitationId });
        return Ok(result);
    }

    [HttpPost("partner-invites/{invitationId}/reject")]
    [Authorize]
    public async Task<ActionResult> RejectPartnerInvite(Guid invitationId)
    {
        var userId = GetUserIdFromToken();
        var result = await _mediator.Send(new RejectPartnerInviteCommand { UserId = userId, InvitationId = invitationId });
        return Ok(result);
    }

    [HttpPost("partner-invites/{invitationId}/cancel")]
    [Authorize]
    public async Task<ActionResult> CancelPartnerInvite(Guid invitationId)
    {
        var userId = GetUserIdFromToken();
        var result = await _mediator.Send(new CancelPartnerInviteCommand { UserId = userId, InvitationId = invitationId });
        return Ok(result);
    }

    [HttpGet("test-auth")]
    [Authorize]
    public ActionResult TestAuth()
    {
        return Ok(new { message = "Auth working!", userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value });
    }

    [HttpGet("debug")]
    public ActionResult DebugToken()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(new { claims });
    }

    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        throw new UnauthorizedAccessException("Invalid user token");
    }
}
