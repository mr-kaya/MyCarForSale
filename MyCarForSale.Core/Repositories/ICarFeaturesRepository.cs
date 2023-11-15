using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Core.Repositories;

public interface ICarFeaturesRepository : IGenericRepository<CarFeaturesEntity>
{
    Task<List<CarFeaturesEntity>> GetCarWithClass();
    Task<List<CarFeaturesEntity>> GetCarWithImages();
    Task<List<CarFeaturesEntity>> GetCarAllClass();
    Task<CarFeaturesEntity> GetCarWithId(int id);
    void UpdateSaleCarInformation(CarFeaturesEntity entity);
    void DeleteSaleCarInformation(CarFeaturesEntity entity);
}