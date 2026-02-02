using MediatR;
using Ceyiz.Application.DTOs;

namespace Ceyiz.Application.Features.Trousseau.Queries;

public class GetTrousseauItemsQuery : IRequest<List<TrousseauItemDto>>
{
    public Guid UserId { get; set; }
}

public class GetTrousseauItemByIdQuery : IRequest<TrousseauItemDto?>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
