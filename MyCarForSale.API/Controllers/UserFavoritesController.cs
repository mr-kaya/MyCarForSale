using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;
using MyCarForSale.Core.Services;

namespace MyCarForSale.API.Controllers;

public class UserFavoritesController : CustomBaseController
{
    private readonly IMapper _mapper;
    private readonly IGenericService<UserFavoritesEntity> _service;


    public UserFavoritesController(IMapper mapper, IGenericService<UserFavoritesEntity> service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var allFavorites = await _service.GetAllAsyncTask();
        var favoritesDto = _mapper.Map<List<UserFavoritesEntityDto>>(allFavorites.ToList());
        return CreateActionResult(CustomResponseDto<List<UserFavoritesEntityDto>>.Success(200, favoritesDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var favorite = await _service.GetByIdAsyncTask(id);
        var favoriteDto = _mapper.Map<UserFavoritesEntityDto>(favorite);
        return CreateActionResult(CustomResponseDto<UserFavoritesEntityDto>.Success(200, favoriteDto));
    }
    
    /*
     * En son eklenen kısım. Burada kullanıcının bütün favorileri sıralanıyor.
     */
    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetIdWhereFavorites(int id)
    {
        var allFavorites = _service.Where(entity => entity.FavoriteUserId == id);
        var allFavoritesDto = _mapper.Map<List<UserFavoritesEntityDto>>(allFavorites);
        return CreateActionResult(CustomResponseDto<List<UserFavoritesEntityDto>>.Success(200, allFavoritesDto));
    }

    [HttpPost]
    public async Task<IActionResult> Save(UserFavoritesEntityDto favoritesEntityDto)
    {
        var mapFavorite = _mapper.Map<UserFavoritesEntity>(favoritesEntityDto);
        await _service.AddAsyncTask(mapFavorite);
        var mapFavoriteDto = _mapper.Map<UserFavoritesEntityDto>(mapFavorite);
        return CreateActionResult(CustomResponseDto<UserFavoritesEntityDto>.Success(201, mapFavoriteDto));
    }

    /* Kullanıcı Favorilerinde Update Gerekmez.
    [HttpPut]
    public async Task<IActionResult> Update(UserFavoritesEntityDto userFavoritesEntityDto)
    {
        var favorite = _mapper.Map<UserFavoritesEntity>(userFavoritesEntityDto);
        await _service.UpdateAsyncTask(favorite);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }
    */

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteFavorite = await _service.GetByIdAsyncTask(id);
        await _service.DeleteAsyncTask(deleteFavorite);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }
}