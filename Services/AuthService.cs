using AssignmentManagement.Data;
using AssignmentManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace AssignmentManagement.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
     public AuthService(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }


   public async Task<ApiResponse> RegisterAsync(RegisterRequest request)
{
    string fullName = request.FullName.Trim();
    string email = request.Email.Trim().ToLower();
    string roleName = request.Role.Trim();

    bool emailExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == email);

    if (emailExists)
    {
        return new ApiResponse
        {
            Success = false,
            Message = "Email already exists."
        };
    }

    if (!Enum.TryParse<UserRole>(roleName, true, out var role))
    {
        return new ApiResponse
        {
            Success = false,
            Message = "Invalid role."
        };
    }

    string passwordHash = _passwordHasher.HashPassword(request.Password);

    User user = new User
    {
        FullName = fullName,
        Email = email,
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
    public async Task<ApiResponse> LoginAsync(LoginRequest request)
    {
        string email = request.Email.Trim().ToLower();

        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if(user == null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }
        bool isPasswordCorrect = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordCorrect)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }
        

        string token = _jwtTokenService.GenerateToken(user);

        return new ApiResponse
        {
            Success = true,
            Message = "Login Successful",
            Data = new
            {
                Token = token,
                User = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    Role = user.Role.ToString()
                }
            }
        };
    }

}
