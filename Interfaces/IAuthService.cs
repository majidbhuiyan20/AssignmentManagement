

namespace AssignmentManagement.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);

    Task<bool> LoginAsync(LoginRequest request);
}