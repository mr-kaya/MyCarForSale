using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Core.Services;

public interface ICarFeaturesService : IGenericService<CarFeaturesEntity>
{
    Task<List<CarFeaturesWithMainClassDto>> GetCarWithClass();
}