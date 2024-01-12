using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Core.Services;

public interface ICarFeaturesService : IGenericService<CarFeaturesEntity>
{
    Task<List<CarFeaturesWithMainClassDto>> GetCarWithClass();
    Task<List<CarFeaturesWithImagesDto>> GetCarWithImages();

    Task<IEnumerable<CarFeaturesWithImageAndClassificationAndUserAccountDto>> GetAllCars();
    Task<CarFeaturesWithImageAndClassificationAndUserAccountDto> GetCarWithId(int id);
    Task<IEnumerable<CarFeaturesWithImageAndClassificationAndUserAccountDto>> GetCarPageWithId(int pageIndex, int pageSize);
    
    Task UpdateSaleCarInformation(CarFeaturesWithImageAndClassificationDto entity);
    Task DeleteSaleCarInformation(CarFeaturesWithImagesDto entity);
}