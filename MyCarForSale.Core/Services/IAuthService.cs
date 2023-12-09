using MyCarForSale.Core.DTOs;

namespace MyCarForSale.Core.Services;

public interface IAuthService
{
    public Task<JwtSettings> LoginUserAsync(UserAccountEntityDto request);
}