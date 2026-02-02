using MediatR;
using Ceyiz.Application.DTOs;

namespace Ceyiz.Application.Features.Trousseau.Commands;

public class CreateTrousseauItemCommand : IRequest<TrousseauItemDto>
{
    public CreateTrousseauItemDto Item { get; set; } = null!;
    public Guid UserId { get; set; }
}

public class UpdateTrousseauItemCommand : IRequest<TrousseauItemDto>
{
    public Guid Id { get; set; }
    public UpdateTrousseauItemDto Item { get; set; } = null!;
    public Guid UserId { get; set; }
}

public class DeleteTrousseauItemCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
