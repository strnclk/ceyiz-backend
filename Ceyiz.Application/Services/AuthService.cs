using Microsoft.EntityFrameworkCore;
using Ceyiz.Persistence.Context;
using Ceyiz.Domain.Entities;
using Ceyiz.Application.DTOs;

namespace Ceyiz.Application.Services;

public interface IAuthService
{
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto> CreateUserAsync(RegisterRequestDto request);
    Task<bool> VerifyPasswordAsync(string email, string password);
}

public class AuthService : IAuthService
{
    private readonly CeyizDbContext _context;

    public AuthService(CeyizDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users
            .Where(u => u.Email == email)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            })
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<UserDto> CreateUserAsync(RegisterRequestDto request)
    {
        // Hash password (basit hashleme - production'da BCrypt kullanın)
        var passwordHash = System.Text.Encoding.UTF8.GetBytes(request.Password);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = Convert.ToBase64String(passwordHash),
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<bool> VerifyPasswordAsync(string email, string password)
    {
        var user = await _context.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();

        if (user == null) return false;

        var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        return user.PasswordHash == passwordHash;
    }
}
