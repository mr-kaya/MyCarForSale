using System.Text.Json;
using MyCarForSale.Core.DTOs;

namespace MyCarForSale.Web.Services;

public class UserAccountService
{
    private readonly HttpClient _httpClient;

    public UserAccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserAccountEntityDto> LoginAccountAsync(string userEmail, string userPassword)
    {
        try
        {
            var response =
                await _httpClient.GetFromJsonAsync<CustomResponseDto<UserAccountEntityDto>>(
                    $"UserAccount/LoginUser?userEmail={userEmail}&userPassword={userPassword}");
            
            if (response is { Erorrs: null })
            {
                return response.Data;     
            }
            
            return null!;
        }
        catch (Exception e)
        {
            return null!;
        }
    }
    
}