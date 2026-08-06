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


    public async Task RegisterAsync(RegisterRequest request)
    {
        // Logic will come here
    
    }


    public async Task<bool> LoginAsync(LoginRequest request)
    {
        // Logic will come here

        return true;
    }

}