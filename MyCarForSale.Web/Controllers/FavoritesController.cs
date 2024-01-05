using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Services;

namespace MyCarForSale.Web.Controllers;

public class FavoritesController : Controller
{
    private readonly FavoritesService _favoritesService;

    public FavoritesController(FavoritesService favoritesService)
    {
        _favoritesService = favoritesService;
    }
    
    
    public async Task<IActionResult> MyFavoritesPage()
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey) as JwtSecurityToken;
        List<CarFeaturesWithImageAndClassificationAndUserAccountDto>? userAllFavorites = null;
        
        if (jsonToken != null)
        {
            var claims = jsonToken.Claims;
            var idClaims = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (idClaims != null) userAllFavorites = await _favoritesService.GetUserAllFavoritesAsync(int.Parse(idClaims));
        }

        return View(userAllFavorites);
    }

    public async Task<IActionResult> DeleteFavoriteId(int id)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey) as JwtSecurityToken;

        if (jsonToken != null)
        {
            var claims = jsonToken.Claims;
            var idClaims = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if (idClaims != null) await _favoritesService.DeleteGetCarId(id.ToString(), idClaims);
        }
        
        return RedirectToAction("MyFavoritesPage", "Favorites");
    }

    public async Task<IActionResult> AddFavorites(string carId)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(UserController.TokenKey) as JwtSecurityToken;
        
        if (jsonToken != null)
        {
            int carIdInt32 = Int32.Parse(carId);
            
            var claims = jsonToken.Claims;
            var idClaims = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (idClaims != null)
            {
                var myFavoritesNumbers = await _favoritesService.GetUserAllFavoriteNumbersAsync(idClaims);
                if (myFavoritesNumbers != null)
                {
                    foreach (var item in myFavoritesNumbers)
                    {
                        if (item.FavoriteBaseId.ToString() == carId)
                        {
                            await _favoritesService.DeleteGetCarId(carId, idClaims);
                        }
                    }
                    
                    /*
                    foreach (var item in myFavoritesNumbers.Select(dto => dto.FavoriteBaseId.ToString()).Where(s => s == carId))
                    {
                        await _favoritesService.DeleteGetCarId(carId, item);
                    } 
                    */  
                }

                UserFavoritesEntityDto userFavoritesEntityDto = new()
                {
                    FavoriteUserId = int.Parse(idClaims),
                    FavoriteBaseId = carIdInt32
                };
                await _favoritesService.PostFavorite(userFavoritesEntityDto);    
            }
        }

        return Redirect($"/Home/CarId/{carId}");
    }
}