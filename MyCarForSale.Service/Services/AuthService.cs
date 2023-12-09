using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Services;

namespace MyCarForSale.Service.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;

    public AuthService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<JwtSettings> LoginUserAsync(UserAccountEntityDto request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            throw new ArgumentNullException(nameof(request));
        }

        var generatedTokenKey = await _tokenService.GenerateToken(request);

        
        return await Task.FromResult(generatedTokenKey);
    }
}