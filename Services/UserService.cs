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
}