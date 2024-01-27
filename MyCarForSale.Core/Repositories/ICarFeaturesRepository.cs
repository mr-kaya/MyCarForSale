using System.Linq.Expressions;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Core.Repositories;

public interface ICarFeaturesRepository : IGenericRepository<CarFeaturesEntity>
{
    Task<List<CarFeaturesEntity>> GetCarWithClass();
    Task<List<CarFeaturesEntity>> GetCarWithImages();

    Task<IEnumerable<CarFeaturesEntity>> GetAllCars();
    Task<CarFeaturesEntity> GetCarWithId(int id);
    Task<List<CarFeaturesEntity>> GetCarListWhere(Expression<Func<CarFeaturesEntity, bool>> expression, int pageIndex, int pageSize);
    Task<List<CarFeaturesEntity>> GetCarWithPageId(int pageIndex, int pageSize);
    void UpdateSaleCarInformation(CarFeaturesEntity entity);
    void DeleteSaleCarInformation(CarFeaturesEntity entity);
}