using AssignmentManagement.Data;
using AssignmentManagement.DTOs.Users;
using AssignmentManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssignmentManagement.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(user => new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<UserResponse?> GetUserByIdAsync(int id)
{
    return await _context.Users
        .Where(user => user.Id == id)
        .Select(user => new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        })
        .FirstOrDefaultAsync();
}
public async Task<UserResponse?> UpdateUserAsync(
    int id,
    UpdateUserRequest request)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == id);

    if (user == null)
    {
        return null;
    }

    bool emailExists = await _context.Users
        .AnyAsync(u =>
            u.Email == request.Email &&
            u.Id != id);

    if (emailExists)
    {
        throw new Exception("Email already exists.");
    }

    if (!Enum.TryParse<UserRole>(
        request.Role,
        true,
        out var role))
    {
        throw new Exception("Invalid role.");
    }

    user.FullName = request.FullName;
    user.Email = request.Email;
    user.Role = role;
    user.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return new UserResponse
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role.ToString(),
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    };
}
}