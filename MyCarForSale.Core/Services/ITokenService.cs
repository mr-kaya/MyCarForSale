using MyCarForSale.Core.DTOs;

namespace MyCarForSale.Core.Services;

public interface ITokenService
{
    public Task<JwtSettings> GenerateToken(UserAccountEntityDto request);
    public bool ValidateToken(string token);
}