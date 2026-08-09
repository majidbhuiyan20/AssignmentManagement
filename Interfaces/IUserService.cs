using AssignmentManagement.DTOs.Users;

namespace AssignmentManagement.Interfaces;

public interface IUserService
{
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse?> GetUserByIdAsync(int id);
    Task<UserResponse?> UpdateUserAsync(
    int id,
    UpdateUserRequest request);
    Task<bool> DeleteUserAsync(int id);
}