using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Service.Exceptions;
using MyCarForSale.Web.Controllers;

namespace MyCarForSale.Web.Services;

public class UserAccountService
{
    private readonly HttpClient _httpClient;

    public UserAccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JwtSettings> LoginAccountAsync(string userEmail, string userPassword)
    {
        var encodedEmail = Uri.EscapeDataString(userEmail);
        var encodedPassword = Uri.EscapeDataString(userPassword);
        var apiUrl = $"UserAccount/LoginUser?userEmail={encodedEmail}&userPassword={encodedPassword}";
        
        try
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<JwtSettings>>(apiUrl);
            
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
    
    public async Task<UserAccountEntityDto> GetUserById()
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+UserController.TokenKey);

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        if (tokenUserId != null)
        {
            var varId = Uri.EscapeDataString(tokenUserId);
            var apiUrl = $"UserAccount/{varId}";

            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<UserAccountEntityDto>>(apiUrl);
            if (response is { Erorrs:null })
            {
                return response.Data;
            }
        }


        throw new NotFoundException("Valid user not found.");
    }
    
}