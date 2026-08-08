using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AssignmentManagement.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace AssignmentManagement.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    public JwtTokenService(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
    }
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
