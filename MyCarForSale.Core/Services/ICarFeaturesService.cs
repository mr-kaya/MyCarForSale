using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Core.Services;

public interface ICarFeaturesService : IGenericService<CarFeaturesEntity>
{
    Task<List<CarFeaturesWithMainClassDto>> GetCarWithClass();
    Task<List<CarFeaturesWithImagesDto>> GetCarWithImages();
    Task<List<CarFeaturesWithClassAndImagesDto>> GetCarAllClass();
    Task<CarFeaturesWithClassAndImagesDto> GetCarWithId(int id);
    Task UpdateSaleCarInformation(CarFeaturesWithClassAndImagesDto entity);
    Task DeleteSaleCarInformation(CarFeaturesWithClassAndImagesDto entity);
}