namespace Ceyiz.Domain.Entities;

public class TrousseauItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? PartnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsCompleted { get; set; }
    public int Quantity { get; set; } = 1;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation property
    public User User { get; set; } = null!;
}
