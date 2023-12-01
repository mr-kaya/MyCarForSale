using AutoMapper;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;
using MyCarForSale.Core.Repositories;
using MyCarForSale.Core.Services;
using MyCarForSale.Core.UnitOfWorks;
using MyCarForSale.Service.Exceptions;

namespace MyCarForSale.Service.Services;

public class CarFeaturesService : GenericService<CarFeaturesEntity>, ICarFeaturesService
{
    private readonly ICarFeaturesRepository _carFeaturesRepository;
    private readonly IMapper _mapper;
    
    public CarFeaturesService(IGenericRepository<CarFeaturesEntity> genericRepository, IUnitOfWork unitOfWork, IMapper mapper, ICarFeaturesRepository carFeaturesRepository) : base(genericRepository, unitOfWork)
    {
        _mapper = mapper;
        _carFeaturesRepository = carFeaturesRepository;
    }

    public async Task<List<CarFeaturesWithMainClassDto>> GetCarWithClass()
    {
         var cars = await _carFeaturesRepository.GetCarWithClass();
         return _mapper.Map<List<CarFeaturesWithMainClassDto>>(cars);
    }

    public async Task<List<CarFeaturesWithImagesDto>> GetCarWithImages()
    {
        var carImages = await _carFeaturesRepository.GetCarWithImages();
        return _mapper.Map<List<CarFeaturesWithImagesDto>>(carImages);
    }

    public async Task<IEnumerable<CarFeaturesWithImageAndClassificationAndUserAccountDto>> GetAllCars()
    {
        var allCars = await _carFeaturesRepository.GetAllCars();
        return _mapper.Map<List<CarFeaturesWithImageAndClassificationAndUserAccountDto>>(allCars);
    }

    public async Task<CarFeaturesWithImageAndClassificationAndUserAccountDto> GetCarWithId(int id)
    {
        var selectedCar = await _carFeaturesRepository.GetCarWithId(id);
        if (selectedCar == null)
        {
            throw new NotFoundException($"Vehicle information not found.");
        }
        
        return _mapper.Map<CarFeaturesWithImageAndClassificationAndUserAccountDto>(selectedCar);
    }

    public async Task UpdateSaleCarInformation(CarFeaturesWithImageAndClassificationDto entity)
    {
        var updateCar = _mapper.Map<CarFeaturesEntity>(entity);
        _carFeaturesRepository.UpdateSaleCarInformation(updateCar);
        
        await _unitOfWork.CommitAsyncTask();
    }

    public async Task DeleteSaleCarInformation(CarFeaturesWithImagesDto entity)
    {
        var carFeaturesEntity = _mapper.Map<CarFeaturesEntity>(entity);
        _carFeaturesRepository.DeleteSaleCarInformation(carFeaturesEntity);
        await _unitOfWork.CommitAsyncTask();
    }
}