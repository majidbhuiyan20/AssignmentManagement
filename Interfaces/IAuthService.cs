

namespace AssignmentManagement.Interfaces;

public interface IAuthService
{
   Task<ApiResponse> RegisterAsync(RegisterRequest request);

    Task<ApiResponse> LoginAsync(LoginRequest request);
}