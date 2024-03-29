﻿using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Controllers;

namespace MyCarForSale.Web.Services;

public class FavoritesService
{
    private readonly HttpClient _httpClient;

    public FavoritesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserFavoritesEntityDto>> GetUserAllFavoriteNumbersAsync(string id)
    {
        var encodeId = Uri.EscapeDataString(id.ToString());
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + UserController.TokenKey);
        var apiUrl = $"UserFavorites/GetIdWhereFavorites/{encodeId}";
        var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<UserFavoritesEntityDto>>>(apiUrl);

        return response?.Data;
    }
    
    public async Task<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>?> GetUserAllFavoritesAsync(int id)
    {
        var carFeaturesEntityDtos =
            new List<CarFeaturesWithImageAndClassificationAndUserAccountDto>();

        var encodeId = Uri.EscapeDataString(id.ToString());
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + UserController.TokenKey);
        var apiUrl = $"UserFavorites/GetIdWhereFavorites/{encodeId}";
        var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<UserFavoritesEntityDto>>>(apiUrl);

        if (response != null)
            foreach (var resData in response.Data)
            {
                encodeId = Uri.EscapeDataString(resData.FavoriteBaseId.ToString());
                apiUrl = $"CarFeatures/GetSaleById/{encodeId}";
                var responseCarFeatures =
                    await _httpClient
                        .GetFromJsonAsync<CustomResponseDto<CarFeaturesWithImageAndClassificationAndUserAccountDto>>(
                            apiUrl);

                if (responseCarFeatures != null) carFeaturesEntityDtos.Add(responseCarFeatures.Data);
            }

        return carFeaturesEntityDtos;
    }

    public async Task<bool> PostFavorite(UserFavoritesEntityDto userFavoritesEntityDto)
    {
        var response = await _httpClient.PostAsJsonAsync("UserFavorites", userFavoritesEntityDto);
        return response.IsSuccessStatusCode;
    }
    
    public async Task DeleteGetCarId(int carId, string userId)
    {
        var encodeCarId = Uri.EscapeDataString(carId.ToString());
        var encodeUserId = Uri.EscapeDataString(userId);

        await _httpClient.DeleteAsync($"UserFavorites/DeleteFavorite?carId={encodeCarId}&userId={encodeUserId}");
    }
}