using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;
using MyCarForSale.Core.Services;

namespace MyCarForSale.API.Controllers;

public class MainPageFilterVariable
{
    public List<string> carClassification { get; set; }
    public int  pageIndex { get; set; }
    public int pageSize { get; set; }
}

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

    [HttpPost("[action]")]
    public async Task<IActionResult> GetSaleByPageId([FromBody] MainPageFilterVariable model)
    {
        IEnumerable<CarFeaturesWithImageAndClassificationAndUserAccountDto>? data = null;
        
        switch (model.carClassification.Count)
        {
            case 0:
                data = await _carFeaturesService.GetCarPageWithId(model.pageIndex, model.pageSize);
                break;
            case 1:
                data = await _carFeaturesService.GetCarListWhere(
                    entity => entity.MainClassificationEntity!.MainClassification == model.carClassification[0], model.pageIndex,
                    model.pageSize);
                break;
            case 2:
                data = await _carFeaturesService.GetCarListWhere(entity =>
                    entity.MainClassificationEntity!.MainClassification == model.carClassification[0] &&
                    entity.MainClassificationEntity.CarBrand == model.carClassification[1], model.pageIndex, model.pageSize);
                break;
            case 3: 
                data = await _carFeaturesService.GetCarListWhere(entity =>
                    entity.MainClassificationEntity!.MainClassification == model.carClassification[0] &&
                    entity.MainClassificationEntity.CarBrand == model.carClassification[1] &&
                    entity.MainClassificationEntity.CarModel == model.carClassification[2], model.pageIndex, model.pageSize);
                break;
            case 4:
                data = await _carFeaturesService.GetCarListWhere(entity =>
                    entity.MainClassificationEntity!.MainClassification == model.carClassification[0] &&
                    entity.MainClassificationEntity.CarBrand == model.carClassification[1] &&
                    entity.MainClassificationEntity.CarModel == model.carClassification[2] &&
                    entity.MainClassificationEntity.CarPackage == model.carClassification[3], model.pageIndex, model.pageSize);
                break;
            default:
                data = await _carFeaturesService.GetCarListWhere(entity =>
                    entity.MainClassificationEntity!.MainClassification == model.carClassification[0] &&
                    entity.MainClassificationEntity.CarBrand == model.carClassification[1] &&
                    entity.MainClassificationEntity.CarModel == model.carClassification[2] &&
                    entity.MainClassificationEntity.CarPackage == model.carClassification[3] &&
                    entity.MainClassificationEntity.CarYear == ushort.Parse(model.carClassification[4]), model.pageIndex, model.pageSize);
                break;
        }
        
        var itemsDto = _mapper.Map<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>(data);
        return CreateActionResult(CustomResponseDto<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>.Success(200, itemsDto));
    }
    
    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetSaleById(int id)
    {
        var sale = await _carFeaturesService.GetCarWithId(id);
        return CreateActionResult(CustomResponseDto<CarFeaturesWithImageAndClassificationAndUserAccountDto>.Success(200, sale));
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetMySalesCarWithMyId(int id)
    {
        var sales = await _carFeaturesService.GetMyCarsWithMyId(id);

        return CreateActionResult(
            CustomResponseDto<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>.Success(200, sales));
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

    [HttpGet("[action]")]
    public async Task<IActionResult> Search(string data)
    {
        var getAllData = _carFeaturesService.Where(entity => entity.UserAccountEntity != null && entity.UserAccountEntity.Name.Contains(data))
            .ToList();

        getAllData.AddRange(_carFeaturesService.Where(entity => entity.AdvertisementName.Contains(data)).ToList());
        
        var getAllDataDto = _mapper.Map<List<CarFeaturesEntityDto>>(getAllData);
        return CreateActionResult(CustomResponseDto<List<CarFeaturesEntityDto>>.Success(200,getAllDataDto));
    }
}