using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Services;

namespace MyCarForSale.Service.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly JwtSettings _jwtSettings;

    public TokenService(IConfiguration configuration, IOptions<JwtSettings>  jwtSettings)
    {
        _configuration = configuration;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<JwtSettings> GenerateToken(UserAccountEntityDto request)
    {
        /*
        SymmetricSecurityKey symmetricSecurityKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
        var dateTimeNow = DateTime.UtcNow;

        JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: new List<Claim>
            {
                new Claim(ClaimTypes.Email, request.Email)
            },
            notBefore: dateTimeNow,
            expires: dateTimeNow.Add(TimeSpan.FromDays(8)),
            signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
        );
        */

        /*
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
        var dateTimeNow = DateTime.UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, request.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(8),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            NotBefore = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        
        return await Task.FromResult(new UserAccountJwt
        {
            AuthToken = tokenHandler.WriteToken(token),
            AccessTokenExpireDate = dateTimeNow.Add(TimeSpan.FromDays(8))
        });
        */

        if (_jwtSettings.Key == null) throw new Exception("Jwt ayarlarındaki Key değeri null olamaz");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsList = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, request.Email),
            new Claim(ClaimTypes.Role, request.Authorization)
        };

        var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claimsList,
            expires: DateTime.UtcNow.AddDays(1), signingCredentials: credentials);

        return await Task.FromResult(new JwtSettings
        {
            Key = new JwtSecurityTokenHandler().WriteToken(token),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        });
    }

    public bool ValidateToken(string token)
    {
        var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        try
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secureKey,
                ValidateLifetime = true,
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}