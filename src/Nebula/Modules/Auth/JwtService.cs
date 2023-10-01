using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nebula.Modules.Auth;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims, int expirationMinutes = 15);
    ClaimsPrincipal? ValidateToken(string token);
}

public class JwtService : IJwtService
{
    private readonly string _secretKey;

    public JwtService(IConfiguration configuration)
    {
        string keygen = "VtHFMgeZ83OzGNvf2TQLSCrWiRcBEbKm";
        _secretKey = configuration.GetValue<string>("SecretKey") ?? keygen;
    }

    public string GenerateToken(IEnumerable<Claim> claims, int expirationMinutes = 15)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            issuer: "http://localhost:5042",
            audience: "NEBULA",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:5042", // Reemplaza con tu emisor
                ValidateAudience = true,
                ValidAudience = "NEBULA", // Reemplaza con tu audiencia
                ClockSkew = TimeSpan.Zero // Permite a los tokens que hayan expirado en el tiempo de expiración (sin tolerancia de reloj)
            }, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            return null; // El token no es válido
        }
    }

}
