using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Controllers;

namespace MyCarForSale.Web.Services;

public class CarFeaturesWithImageAndClassificationAndUserAccountService
{
    private readonly HttpClient _httpClient;

    public CarFeaturesWithImageAndClassificationAndUserAccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>> AllSaleCarsAsync()
    {
        var response =
            await _httpClient
                .GetFromJsonAsync<CustomResponseDto<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>>(
                    "CarFeatures/AllSaleCars");
        
        return response.Data;
    }

    public async Task<List<MainClassificationEntityDto>> AllClassificationAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<MainClassificationEntityDto>>>("MainClassification");
        return response.Data;
    }
    
    public async Task<CarFeaturesEntityDto> SaveAsync(CarFeaturesEntityDto newCarFeatures)
    {
        var response = await _httpClient.PostAsJsonAsync("CarFeatures", newCarFeatures);

        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<CarFeaturesEntityDto>>();
        return responseBody.Data;
    }

    public async Task<CarFeaturesWithImageAndClassificationAndUserAccountDto> GetSaleWithIdAsync(int id)
    {
        var response =
            await _httpClient
                .GetFromJsonAsync<CustomResponseDto<CarFeaturesWithImageAndClassificationAndUserAccountDto>>(
                    $"CarFeatures/GetSaleById/{id}");

        if (response == null && response.Erorrs.Any())
        {
            return null;
        }
        return response.Data;
    }

    public async Task<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>? GetSalePageWithId(MainPageFilterVariable model)
    {
        var response =
            await _httpClient
                .PostAsJsonAsync($"CarFeatures/GetSaleByPageId", model);

        var i = model.carClassification.Count;
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content
                .ReadFromJsonAsync<CustomResponseDto<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>>();

            return result.Data;
        }
        
        return null;

        //var pageIndexUri = Uri.EscapeDataString(pageIndex.ToString());
        //var pageSizeUri = Uri.EscapeDataString(pageSize.ToString());

        /*
         var response =
            await _httpClient
                .PostAsJsonAsync<CustomResponseDto<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>>(
                    $"CarFeatures/GetSaleByPageId?pageIndex={pageIndexUri}&pageSize={pageSizeUri}");

        return response.Data;
         */
    }
    
    public async Task<bool> UpdateAsync(CarFeaturesEntityDto newCarFeatures)
    {
        var response = await _httpClient.PutAsJsonAsync("CarFeatures", newCarFeatures);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"CarFeatures/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<CarFeaturesEntityDto>> SearchCarFeaturesDto(string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            var response =
                await _httpClient.GetFromJsonAsync<CustomResponseDto<List<CarFeaturesEntityDto>>>($"CarFeatures/Search?data={data}");
            return response.Data;
        }

        return null;
    }

    public async Task<CarFeaturesWithImageAndClassificationAndUserAccountDto> SearchCarImageDto(CarFeaturesEntityDto carFeaturesEntityDto)
    {
        var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<CarFeaturesWithImageAndClassificationAndUserAccountDto>>($"CarFeatures/GetSaleById/{carFeaturesEntityDto.Id}");

        return response?.Data;
    }
}