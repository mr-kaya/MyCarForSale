using Microsoft.EntityFrameworkCore;
using MyCarForSale.Core.Entities;
using MyCarForSale.Core.Repositories;

namespace MyCarForSale.Repository.Repositories;

public class CarFeaturesRepository : GenericRepository<CarFeaturesEntity>, ICarFeaturesRepository
{
    public CarFeaturesRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<CarFeaturesEntity>> GetCarWithClass()
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.MainClassificationEntity).ToListAsync();
    }

    public async Task<List<CarFeaturesEntity>> GetCarWithImages()
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.CarImagesEntities).ToListAsync();
    }

    public async Task<List<CarFeaturesEntity>> GetCarAllClass()
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.MainClassificationEntity)
            .Include(y => y.CarImagesEntities).Include(z=>z.UserAccountEntity).ToListAsync();
    }

    public async Task<CarFeaturesEntity> GetCarWithId(int id)
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.MainClassificationEntity)
            .Include(y => y.CarImagesEntities).Include(z => z.UserAccountEntity)
            .FirstOrDefaultAsync(x => x.MainClassificationEntityId == id);
    }
}