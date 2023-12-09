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

    public async Task<UserAccountJwt> LoginAccountAsync(string userEmail, string userPassword)
    {
        var encodedEmail = Uri.EscapeDataString(userEmail);
        var encodedPassword = Uri.EscapeDataString(userPassword);
        var apiUrl = $"UserAccount/LoginUser?userEmail={encodedEmail}&userPassword={encodedPassword}";
        
        try
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<UserAccountJwt>>(apiUrl);
            
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

    public async Task<List<UserAccountEntityDto>> AllUserDeneme()
    {
        var apiUrl = $"UserAccount/All";
        var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<UserAccountEntityDto>>>(apiUrl);

        return response!.Data;
    }
    
}