

namespace AssignmentManagement.Interfaces;

public interface IAuthService
{
   Task<ApiResponse> RegisterAsync(RegisterRequest request);

    Task<bool> LoginAsync(LoginRequest request);
}