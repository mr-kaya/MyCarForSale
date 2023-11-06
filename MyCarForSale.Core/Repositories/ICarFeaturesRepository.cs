using MyCarForSale.Core.Entities;

namespace MyCarForSale.Core.Repositories;

public interface ICarFeaturesRepository : IGenericRepository<CarFeaturesEntity>
{
    Task<List<CarFeaturesEntity>> GetCarWithClass();
}