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
    public async Task<IActionResult> GetSaleByPageId(int pageIndex, int pageSize)
    {
        var items = await _carFeaturesService.GetCarPageWithId(pageIndex, pageSize);
        var itemsDto = _mapper.Map<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>(items);
        return CreateActionResult(CustomResponseDto<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>.Success(200, itemsDto));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetFindClassification(string[] carClassification)
    {
        IQueryable<CarFeaturesEntity> data;
        
        if (carClassification.Length == 1)
        {
            data = _service.Where(entity =>
                entity.MainClassificationEntity!.MainClassification == carClassification[0]);
        }
        else if (carClassification.Length == 2)
        {
            data = _service.Where(entity =>
                entity.MainClassificationEntity!.MainClassification == carClassification[0] &&
                entity.MainClassificationEntity.CarBrand == carClassification[1]);
        }
        else if (carClassification.Length == 3)
        {
            data = _service.Where(entity =>
                entity.MainClassificationEntity!.MainClassification == carClassification[0] &&
                entity.MainClassificationEntity.CarBrand == carClassification[1] &&
                entity.MainClassificationEntity.CarModel == carClassification[2]);
        }
        else if(carClassification.Length == 4)
        {
            data = _service.Where(entity =>
                entity.MainClassificationEntity!.MainClassification == carClassification[0] &&
                entity.MainClassificationEntity.CarBrand == carClassification[1] &&
                entity.MainClassificationEntity.CarModel == carClassification[2] &&
                entity.MainClassificationEntity.CarPackage == carClassification[3]);
        }
        else
        {
            data = _service.Where(entity =>
                entity.MainClassificationEntity!.MainClassification == carClassification[0] &&
                entity.MainClassificationEntity.CarBrand == carClassification[1] &&
                entity.MainClassificationEntity.CarModel == carClassification[2] &&
                entity.MainClassificationEntity.CarPackage == carClassification[3] &&
                entity.MainClassificationEntity.CarYear == ushort.Parse(carClassification[4]));
        }

        var dataDto = _mapper.Map<List<CarFeaturesEntityDto>>(data);
        
        return CreateActionResult(CustomResponseDto<List<CarFeaturesEntityDto>>.Success(200, dataDto));
    }
    
    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetSaleById(int id)
    {
        var sale = await _carFeaturesService.GetCarWithId(id);
        return CreateActionResult(CustomResponseDto<CarFeaturesWithImageAndClassificationAndUserAccountDto>.Success(200, sale));
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

    [HttpGet("[action]")]
    public async Task<IActionResult> AllSaleCars()
    {
        var allSaleCars = await _carFeaturesService.GetAllCars();
        
        return CreateActionResult(CustomResponseDto<IEnumerable<CarFeaturesWithImageAndClassificationAndUserAccountDto>>.Success(200, allSaleCars));
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

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetWhereTask(int id)
    {
        var whereData = _carFeaturesService.Where(entity => entity.PublishUserId == id);
        var whereDataDto = _mapper.Map<List<CarFeaturesEntityDto>>(whereData);
        return CreateActionResult(CustomResponseDto<List<CarFeaturesEntityDto>>.Success(200, whereDataDto));
    }
    
    [HttpPost]
    public async Task<IActionResult> Save(CarFeaturesEntityDto carFeaturesEntityDto)
    {
        var car = _mapper.Map<CarFeaturesEntity>(carFeaturesEntityDto);
         await _service.AddAsyncTask(car);
        var carDto = _mapper.Map<CarFeaturesEntityDto>(car);
        return CreateActionResult(CustomResponseDto<CarFeaturesEntityDto>.Success(201, carDto));
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateSaleCar(CarFeaturesWithImageAndClassificationDto carFeaturesWithImagesDto)
    {
        await _carFeaturesService.UpdateSaleCarInformation(carFeaturesWithImagesDto);
        
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(CarFeaturesEntityDto carFeaturesEntityDto)
    {
        var car = _mapper.Map<CarFeaturesEntity>(carFeaturesEntityDto);
        await _service.UpdateAsyncTask(car);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteSaleCar(int id)
    {
        var car = await _carFeaturesService.GetCarWithId(id);
        var carImageDto = _mapper.Map<CarFeaturesWithImagesDto>(car);
        await _carFeaturesService.DeleteSaleCarInformation(carImageDto);
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