using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;
using MyCarForSale.Core.Services;

namespace MyCarForSale.API.Controllers;

public class CarImagesController : CustomBaseController
{
    private readonly IMapper _mapper;
    private readonly IGenericService<CarImagesEntity> _service;
    
    public CarImagesController(IMapper mapper, IGenericService<CarImagesEntity> service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var images = await _service.GetAllAsyncTask();
        var imagesDto = _mapper.Map<List<CarImagesEntityDto>>(images.ToList());
        return CreateActionResult(CustomResponseDto<List<CarImagesEntityDto>>.Success(200, imagesDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var image = await _service.GetByIdAsyncTask(id);
        var imageDto = _mapper.Map<CarImagesEntityDto>(image);
        return CreateActionResult(CustomResponseDto<CarImagesEntityDto>.Success(200, imageDto));
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetCarPicturesWithCarId(int id)
    {
        var images = await _service.Where(entity => entity.BaseEntityId == id).ToListAsync();
        var imagesDto = _mapper.Map<List<CarImagesEntityDto>>(images);
        return CreateActionResult(CustomResponseDto<List<CarImagesEntityDto>>.Success(200, imagesDto));
    }
    
    [HttpPost]
    public async Task<IActionResult> Save(CarImagesEntityDto carImagesEntityDto)
    {
        var image = _mapper.Map<CarImagesEntity>(carImagesEntityDto);
        await _service.AddAsyncTask(image);
        var imageDto = _mapper.Map<CarImagesEntityDto>(image);
        return CreateActionResult(CustomResponseDto<CarImagesEntityDto>.Success(201, imageDto));
    }

    [HttpPut]
    public async Task<IActionResult> Update(CarImagesEntityDto carImagesEntityDto)
    {
        var image = _mapper.Map<CarImagesEntity>(carImagesEntityDto);
        await _service.UpdateAsyncTask(image);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var selectImage = await _service.GetByIdAsyncTask(id);
        await _service.DeleteAsyncTask(selectImage);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }

    [HttpDelete("[Action]/{id}")]
    public async Task<IActionResult> DeleteAll(int id)
    {
        var getImageList = await _service.Where(x => x.BaseEntityId == id).ToListAsync();
        await _service.DeleteRangeAsyncTask(getImageList);

        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }
}