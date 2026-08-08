using AssignmentManagement.DTOs.Users;

namespace AssignmentManagement.Interfaces;

public interface IUserService
{
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse?> GetUserByIdAsync(int id);
}