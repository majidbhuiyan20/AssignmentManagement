
namespace AssignmentManagement.Interfaces;
public interface IJwtTokenService
{
    string GenerateToken(User user);
}
