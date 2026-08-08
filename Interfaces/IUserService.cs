using AssignmentManagement.DTOs.Users;

namespace AssignmentManagement.Interfaces;

public interface IUserService
{
    Task<List<UserResponse>> GetAllUsersAsync();
}