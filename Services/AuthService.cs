using AssignmentManagement.Data;
using AssignmentManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace AssignmentManagement.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
     public AuthService(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }


   public async Task<ApiResponse> RegisterAsync(RegisterRequest request)
{
    bool emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email);

    if (emailExists)
    {
        return new ApiResponse
        {
            Success = false,
            Message = "Email already exists."
        };
    }

    string passwordHash = _passwordHasher.HashPassword(request.Password);

    if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
    {
        return new ApiResponse
        {
            Success = false,
            Message = "Invalid role."
        };
    }

    User user = new User
    {
        FullName = request.FullName,
        Email = request.Email,
        PasswordHash = passwordHash,
        Role = role,
        CreatedAt = DateTime.UtcNow
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    return new ApiResponse
    {
        Success = true,
        Message = "User registered successfully."
    };
}
    public async Task<bool> LoginAsync(LoginRequest request)
    {
        // Logic will come here

        return true;
    }

}