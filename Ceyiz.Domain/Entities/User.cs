namespace Ceyiz.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<TrousseauItem> TrousseauItems { get; set; } = new List<TrousseauItem>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public UserProfile? Profile { get; set; }
}
