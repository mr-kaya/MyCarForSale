using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Imagekit.Sdk;
using Microsoft.IdentityModel.JsonWebTokens;
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

    public async Task<bool> UpdateCarFeatures(
        CarFeaturesWithImageAndClassificationDto carFeaturesWithImageAndClassification)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (tokenUserId != null && carFeaturesWithImageAndClassification.PublishUserId == Int32.Parse(tokenUserId))
        {
            var urlString = "CarFeatures/UpdateSaleCar";
            var response = await _httpClient.PutAsJsonAsync(urlString, carFeaturesWithImageAndClassification);

            return response.IsSuccessStatusCode;
        }

        return false;
    }

    public async Task<List<CarImagesEntityDto>> GetCarImagesWithId(int carId, string userId)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (tokenUserId != null)
        {
            if (userId == tokenUserId)  
            {
                var urlString = Uri.EscapeDataString(carId.ToString());
                urlString = "CarImages/GetCarPicturesWithCarId/" + urlString;
                var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<CarImagesEntityDto>>>(urlString);

                if (response is { StatusCode: < 300 }) return response.Data;
            }
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

    public async Task<bool> PostImageWithDatabase(CarImagesEntityDto carImagesEntityDto)
    {
        var response = await _httpClient.PostAsJsonAsync("CarImages", carImagesEntityDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<MainClassificationEntityDto> GetClassificationWithCarId(int carId, int userId)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (tokenUserId != null && tokenUserId == userId.ToString())
        {
            var stringCarId = Uri.EscapeDataString(carId.ToString());
            var url = "MainClassification/" + stringCarId;

            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<MainClassificationEntityDto>>(url);

            if (response.StatusCode < 300)
            {
                return response.Data;
            }
        }

        return null;
    }
    
    public async Task<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>?> GetSingleUserCars()
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+UserController.TokenKey);
        Console.WriteLine("Key = "+UserController.TokenKey);
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (tokenUserId == null)
        {
            return null;
        }
        string url = Uri.EscapeDataString(tokenUserId);
        url = "CarFeatures/GetMySalesCarWithMyId/"+url;

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var result = await _httpClient
                .GetFromJsonAsync<CustomResponseDto<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>>(url);

            return result?.Data;
        }
        return null;
    }

    public async Task<bool> DeleteMySelectCar(int userId, int carId)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (tokenUserId != null)
        {
            var intTokenUserId = Int32.Parse(tokenUserId);
            
            if (intTokenUserId == userId)
            {
                var stringCarId = Uri.EscapeDataString(carId.ToString());
                
                var url = "CarImages/DeleteAll/" + stringCarId;
                var response = await _httpClient.DeleteAsync(url);
 
                if (response.IsSuccessStatusCode)
                {
                    url = "CarFeatures/" + stringCarId;
                    response = await _httpClient.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
        }
        
        return false;
    }

    public async Task<CarFeaturesWithImageAndClassificationAndUserAccountDto> GetSaleCarWithId(int id)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (tokenUserId != null && id != 0)
        {
            var urlString = Uri.EscapeDataString(id.ToString());
            var url = "CarFeatures/GetSaleById/" + urlString;
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CarFeaturesWithImageAndClassificationAndUserAccountDto>>(url);

            if (response != null && response.Data.UserAccountEntityId == tokenUserId)
            {
                return response.Data;
            }
        }

        return null;
    }

    public async Task<CarFeaturesEntityDto> GetCarWithId(int id)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (tokenUserId != null && id != 0)
        {
            var urlString = Uri.EscapeDataString(id.ToString());
            var url = "CarFeatures/" + urlString;
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CarFeaturesEntityDto>>(url);

            if (response != null && response.Data.Id == id)
            {
                return response.Data;
            }
        }

        return null;
    }

    public async Task<bool> DeleteImageWithId(int imageId, int userId)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        
        if (!string.IsNullOrEmpty(tokenUserId) && tokenUserId == userId.ToString())
        {
            string stringImageId = Uri.EscapeDataString(imageId.ToString());
            string url = "CarImages/"+stringImageId;
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }

        return false;
    }
}