namespace api.Services;
public class AuthService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public AuthService(string secretKey, string issuer, string audience)
    {
        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
    }

    public string GetToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(3600),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public Dictionary<string, string> GetUserClaims(ClaimsPrincipal user)
    {
        var claims = new Dictionary<string, string>
        {
            { "UserId", user.FindFirst("UserId")?.Value },
            { "Email", user.FindFirst("Email")?.Value },
            { "Name", user.FindFirst(ClaimTypes.Name)?.Value }
        };

        return claims;
    }
}
