using MediatR;
using Microsoft.EntityFrameworkCore;
using Ceyiz.Persistence.Context;
using Ceyiz.Application.DTOs;
using Ceyiz.Application.Features.Trousseau.Commands;
using Ceyiz.Application.Features.Trousseau.Queries;

namespace Ceyiz.Application.Features.Trousseau.Handlers;

public class GetTrousseauItemsQueryHandler : IRequestHandler<GetTrousseauItemsQuery, List<TrousseauItemDto>>
{
    private readonly CeyizDbContext _context;

    public GetTrousseauItemsQueryHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<List<TrousseauItemDto>> Handle(GetTrousseauItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.TrousseauItems
            .Where(i => i.UserId == request.UserId || i.PartnerId == request.UserId)
            .Select(i => new TrousseauItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Category = i.Category,
                Price = i.Price,
                Quantity = i.Quantity,
                IsCompleted = i.IsCompleted,
                ImageUrl = i.ImageUrl,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return items;
    }
}

public class GetTrousseauItemByIdQueryHandler : IRequestHandler<GetTrousseauItemByIdQuery, TrousseauItemDto?>
{
    private readonly CeyizDbContext _context;

    public GetTrousseauItemByIdQueryHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<TrousseauItemDto?> Handle(GetTrousseauItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _context.TrousseauItems
            .Where(i => i.Id == request.Id && i.UserId == request.UserId)
            .Select(i => new TrousseauItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Category = i.Category,
                Price = i.Price,
                Quantity = i.Quantity,
                IsCompleted = i.IsCompleted,
                ImageUrl = i.ImageUrl,
                CreatedAt = i.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return item;
    }
}

public class CreateTrousseauItemCommandHandler : IRequestHandler<CreateTrousseauItemCommand, TrousseauItemDto>
{
    private readonly CeyizDbContext _context;

    public CreateTrousseauItemCommandHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<TrousseauItemDto> Handle(CreateTrousseauItemCommand request, CancellationToken cancellationToken)
    {
        var item = new Domain.Entities.TrousseauItem
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Name = request.Item.Name,
            Description = request.Item.Description,
            Category = request.Item.Category,
            Price = request.Item.Price,
            Quantity = request.Item.Quantity,
            IsCompleted = false,
            ImageUrl = request.Item.ImageUrl,
            CreatedAt = DateTime.UtcNow
        };

        _context.TrousseauItems.Add(item);
        await _context.SaveChangesAsync(cancellationToken);

        return new TrousseauItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Category = item.Category,
            Price = item.Price,
            Quantity = item.Quantity,
            IsCompleted = item.IsCompleted,
            ImageUrl = item.ImageUrl,
            CreatedAt = item.CreatedAt
        };
    }
}

public class UpdateTrousseauItemCommandHandler : IRequestHandler<UpdateTrousseauItemCommand, TrousseauItemDto?>
{
    private readonly CeyizDbContext _context;

    public UpdateTrousseauItemCommandHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<TrousseauItemDto?> Handle(UpdateTrousseauItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.TrousseauItems
            .FirstOrDefaultAsync(i => i.Id == request.Id && i.UserId == request.UserId, cancellationToken);

        if (item == null)
            return null;

        item.Name = request.Item.Name;
        item.Description = request.Item.Description;
        item.Category = request.Item.Category;
        item.Price = request.Item.Price;
        item.Quantity = request.Item.Quantity;
        item.IsCompleted = request.Item.IsCompleted;
        item.ImageUrl = request.Item.ImageUrl;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new TrousseauItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Category = item.Category,
            Price = item.Price,
            Quantity = item.Quantity,
            IsCompleted = item.IsCompleted,
            ImageUrl = item.ImageUrl,
            CreatedAt = item.CreatedAt
        };
    }
}

public class DeleteTrousseauItemCommandHandler : IRequestHandler<DeleteTrousseauItemCommand, bool>
{
    private readonly CeyizDbContext _context;

    public DeleteTrousseauItemCommandHandler(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTrousseauItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.TrousseauItems
            .FirstOrDefaultAsync(i => i.Id == request.Id && i.UserId == request.UserId, cancellationToken);

        if (item == null)
            return false;

        _context.TrousseauItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
