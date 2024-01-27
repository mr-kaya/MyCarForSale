using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Imagekit.Sdk;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Controllers;

namespace MyCarForSale.Web.Services;

public class MyCarsService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public MyCarsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<MainClassificationEntityDto>> GetAllClassification()
    {
        var response =
            await _httpClient.GetFromJsonAsync<CustomResponseDto<List<MainClassificationEntityDto>>>("MainClassification");
        
        return response.Data;
    }

    public async Task<string> GetUserId()
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+UserController.TokenKey);
        Console.WriteLine("Key = "+UserController.TokenKey);
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        if (tokenUserId != null)
        {
            return tokenUserId;
        }

        return null;
    }
    
    public async Task<MainClassificationEntityDto> PostClassification(string mainClassification, string carBrand, string carModel, string carPackage, ushort carYear)
    {
        MainClassificationEntityDto mainClassificationEntityDto = new()
        {
            MainClassification = mainClassification,
            CarBrand = carBrand,
            CarModel = carModel,
            CarPackage = carPackage,
            CarYear = carYear
        };
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+UserController.TokenKey);
        Console.WriteLine("Key = "+UserController.TokenKey);
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        if (tokenUserId != null)
        {
            var response =
                await _httpClient.PostAsJsonAsync("MainClassification", mainClassificationEntityDto);

            if (response.IsSuccessStatusCode)
            {
                var responseBody =  await response.Content.ReadFromJsonAsync<CustomResponseDto<MainClassificationEntityDto>>();
                mainClassificationEntityDto = responseBody.Data;
            }
        }
        
        return mainClassificationEntityDto;
    }

    public async Task<CarFeaturesEntityDto> PostCarFeatures(CarFeaturesEntityDto carFeaturesEntityDto)
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+UserController.TokenKey);
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (tokenUserId != null)
        {
            var response = await _httpClient.PostAsJsonAsync("CarFeatures", carFeaturesEntityDto);
            
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<CustomResponseDto<CarFeaturesEntityDto>>();
                
                return data.Data;
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Hata oluştu : {errorMessage}");
        }

        return null;
    }
    
    
    public async Task<string> PostImageKitWebsite(string path, string fileName)
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+UserController.TokenKey);
        Console.WriteLine("Key = "+UserController.TokenKey);
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        
        ImagekitClient imageKit = new ImagekitClient(_configuration["ImageKit:PublicKey"], _configuration["ImageKit:PrivateKey"], _configuration["ImageKit:UrlEndPoint"]);

        byte[] bytes = File.ReadAllBytes(path);
        FileCreateRequest ob = new FileCreateRequest
        {
            file = bytes,
            fileName = fileName
        };
        List<string> tags = new List<string>
        {
            tokenUserId!
        };
        ob.tags = tags;
        Result resp = await imageKit.UploadAsync(ob);

        return resp.url;
    }

    public async Task PostImageWithDatabase(CarImagesEntityDto carImagesEntityDto)
    {
        await _httpClient.PostAsJsonAsync("CarImages", carImagesEntityDto);
    }
}