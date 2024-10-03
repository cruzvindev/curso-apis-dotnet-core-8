using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace APICatalogo.Services;

public class TokenService : ITokenService
{
    //Esse método será executado sempre que for necessário gerar um novo JWT
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
    {
        //Obetendo chave secreta definida no appsettings
        var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                  throw new InvalidOperationException("Invalid secret Key");
        
        //convertendo secret key para um array de bytes
        var privateKey = Encoding.UTF8.GetBytes(key);
        
        //Uso a chave para criar as credenciais que vão assinar o token 
        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);
        
        //Definindo os valores que serão usados para criar o token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),
            Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),
            Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return token;
    }

    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(secureRandomBytes);
        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
    {
        //Obtendo chave secreta
        var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid key");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}