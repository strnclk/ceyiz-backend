using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ceyiz.Application.DTOs;
using Ceyiz.Application.Features.Trousseau.Commands;
using Ceyiz.Application.Features.Trousseau.Queries;

namespace Ceyiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrousseauController : ControllerBase
{
    private readonly IMediator _mediator;

    public TrousseauController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<TrousseauItemDto>>> GetItems([FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetTrousseauItemsQuery { UserId = userId });
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrousseauItemDto>> GetItem(Guid id, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetTrousseauItemByIdQuery { Id = id, UserId = userId });
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TrousseauItemDto>> CreateItem([FromBody] CreateTrousseauItemDto item, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new CreateTrousseauItemCommand { Item = item, UserId = userId });
        return CreatedAtAction(nameof(GetItem), new { id = result.Id, userId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TrousseauItemDto>> UpdateItem(Guid id, [FromBody] UpdateTrousseauItemDto item, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new UpdateTrousseauItemCommand { Id = id, Item = item, UserId = userId });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItem(Guid id, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new DeleteTrousseauItemCommand { Id = id, UserId = userId });
        if (result)
            return NoContent();
        
        return BadRequest();
    }
}
