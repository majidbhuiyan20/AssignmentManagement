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
    string fullName = request.FullName.Trim();
    string email = request.Email.Trim().ToLower();
    string roleName = request.Role.Trim();

    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == id);

    if (user == null)
    {
        return null;
    }

    bool emailExists = await _context.Users
        .AnyAsync(u =>
            u.Email.ToLower() == email &&
            u.Id != id);

    if (emailExists)
    {
        throw new InvalidOperationException("Email already exists.");
    }

    if (!Enum.TryParse<UserRole>(
        roleName,
        true,
        out var role))
    {
        throw new InvalidOperationException("Invalid role.");
    }

    user.FullName = fullName;
    user.Email = email;
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
    public async Task<bool> DeleteUserAsync(int id)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == id);

    if (user == null)
    {
        return false;
    }

    _context.Users.Remove(user);

    await _context.SaveChangesAsync();

    return true;
}
}
