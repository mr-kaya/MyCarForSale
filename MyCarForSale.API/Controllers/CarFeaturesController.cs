using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;
using MyCarForSale.Core.Services;

namespace MyCarForSale.API.Controllers;

public class CarFeaturesController : CustomBaseController
{
    private readonly IMapper _mapper;
    private readonly IGenericService<CarFeaturesEntity> _service;
    private readonly ICarFeaturesService _carFeaturesService;
    
    public CarFeaturesController(IMapper mapper, IGenericService<CarFeaturesEntity> service, ICarFeaturesService carFeaturesService)
    {
        _mapper = mapper;
        _service = service;
        _carFeaturesService = carFeaturesService;
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetCarWithImages()
    {
        var cars = await _carFeaturesService.GetCarWithImages();
        return CreateActionResult(CustomResponseDto<List<CarFeaturesWithImagesDto>>.Success(200, cars));
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetCarWithClass()
    {
        var cars = await _carFeaturesService.GetCarWithClass();
        return CreateActionResult(CustomResponseDto<List<CarFeaturesWithMainClassDto>>.Success(200, cars));
    }
    
    [HttpGet]
    public async Task<IActionResult> All()
    {
        var allCars = await _service.GetAllAsyncTask();
        var allCarDtos = _mapper.Map<List<CarFeaturesEntityDto>>(allCars.ToList());
        return CreateActionResult(CustomResponseDto<List<CarFeaturesEntityDto>>.Success(200, allCarDtos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _service.GetByIdAsyncTask(id);
        var carDto = _mapper.Map<CarFeaturesEntityDto>(car);
        return CreateActionResult(CustomResponseDto<CarFeaturesEntityDto>.Success(200, carDto));
    }

    [HttpPost]
    public async Task<IActionResult> Save(CarFeaturesEntityDto carFeaturesEntityDto)
    {
        var car = _mapper.Map<CarFeaturesEntity>(carFeaturesEntityDto);
         await _service.AddAsyncTask(car);
        var carDto = _mapper.Map<CarFeaturesEntityDto>(car);
        return CreateActionResult(CustomResponseDto<CarFeaturesEntityDto>.Success(201, carDto));
    }

    [HttpPut]
    public async Task<IActionResult> Update(CarFeaturesEntityDto carFeaturesEntityDto)
    {
        var car = _mapper.Map<CarFeaturesEntity>(carFeaturesEntityDto);
        await _service.UpdateAsyncTask(car);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _service.GetByIdAsyncTask(id);
        await _service.DeleteAsyncTask(car);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));

    }
}