using System.Globalization;
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
    
    public async Task<UserAccountEntityDto> GetUserById()
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+UserController.TokenKey);
        Console.WriteLine("Key = "+UserController.TokenKey);
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

    public async Task<string> GetValidUserEmail(string email)
    {
        var encodeEmail = Uri.EscapeDataString(email);
        var apiUrl = $"UserAccount/FindEmail?userEmail={encodeEmail}";

        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            return "";
        }
        
        return "This email address is already registered";
    }

    public async Task DeleteUser(int id)
    {
        var encodeId = Uri.EscapeDataString(id.ToString());
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+UserController.TokenKey);
        var apiUrl = $"UserAccount/{encodeId}";
        var response = await _httpClient.DeleteAsync(apiUrl);

    }
    
    public async Task<HttpResponseMessage> PostUser(UserAccountEntityDto userAccountEntityDto)
    {
        var apiUrl = $"UserAccount";
        var response = await _httpClient.PostAsJsonAsync(apiUrl, userAccountEntityDto);

        return response;
    }
    
    public async Task UpdateUser(string name, string surname, string birthday, string phone)
    {
        var getUserValueById = await GetUserById();
        
        getUserValueById.Name = name;
        getUserValueById.Surname = surname;
        
        
        if (new[] {".", ",", "-", "/", " "}.Any(findValue => birthday.Contains(findValue)) && birthday != null)
        {
            getUserValueById.Birthday = DateTime.Parse(birthday, new CultureInfo("tr-TR"));
        }
        getUserValueById.PhoneNumber = phone;
        var apiUrl = $"UserAccount";
        var response = await _httpClient.PutAsJsonAsync(apiUrl, getUserValueById);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new ArgumentException("Failed to update user.");
        }
    }

    public async Task UpdatePassword(string password1, string password2)
    {
        var getUserValueId = await GetUserById();

        if (password1 == password2)
        {
            getUserValueId.Password = password1;
            var apiUrl = $"UserAccount";
            var response = await _httpClient.PutAsJsonAsync(apiUrl, getUserValueId);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Failed to update password.");
            }
        }
    }

    public async Task UpdateAddress(string province, string district, string fullAddress,
        string zipCode)
    {
        Console.WriteLine("değerler = {0} {1} {2} {3}", province, district, fullAddress, zipCode);
        var getUserValueId = await GetUserById();

        getUserValueId.Province = province;
        getUserValueId.District = district;
        getUserValueId.FullAddress = fullAddress;
        getUserValueId.ZipCode = zipCode;

        var apiUrl = $"UserAccount";
        var response = await _httpClient.PutAsJsonAsync(apiUrl, getUserValueId);
        if (!response.IsSuccessStatusCode)
        {
            throw new ArgumentException("Failed to update address.");
        }
    }
}