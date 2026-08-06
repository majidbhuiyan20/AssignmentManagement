using Microsoft.Extensions.Options;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    public JwtTokenService(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
    }
    public string GenerateToken(string userId, string role)
    {
        return string.Empty;
    }
}