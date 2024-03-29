﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyCarForSale.Core.DTOs;
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

    public async Task<IEnumerable<CarFeaturesEntity>> GetAllCars()
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.MainClassificationEntity)
            .Include(y => y.CarImagesEntities).Include(z => z.UserAccountEntity).ToListAsync();
    }

    public async Task<List<CarFeaturesEntity>> GetCarAllClass()
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.MainClassificationEntity)
            .Include(y => y.CarImagesEntities).Include(z=>z.UserAccountEntity).ToListAsync();
    }

    public async Task<CarFeaturesEntity> GetCarWithId(int id)
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.CarImagesEntities)
            .Include(y => y.MainClassificationEntity).Include(z => z.UserAccountEntity).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<CarFeaturesEntity>> GetMyCarsWithMyId(int id)
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.CarImagesEntities)
            .Include(y => y.MainClassificationEntity).Include(z => z.UserAccountEntity)
            .Where(x => x.PublishUserId == id).ToListAsync();
    }

    public async Task<List<CarFeaturesEntity>> GetCarListWhere(Expression<Func<CarFeaturesEntity, bool>> expression, int pageIndex, int pageSize)
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.CarImagesEntities)
            .Include(y => y.MainClassificationEntity).Include(z => z.UserAccountEntity).Where(expression).Skip((pageIndex - 1) * pageSize)
            .Take(pageSize).ToListAsync();
    }

    public async Task<List<CarFeaturesEntity>> GetCarWithPageId(int pageIndex, int pageSize)
    {
        return await _dbContext.FeaturesBaseEntities.Include(x => x.CarImagesEntities)
            .Include(y => y.MainClassificationEntity).Include(z => z.UserAccountEntity).Skip((pageIndex - 1) * pageSize)
            .Take(pageSize).ToListAsync();
    }

    public void UpdateSaleCarInformation(CarFeaturesEntity entity)
    {
        _dbSet.Include(x => x.MainClassificationEntity).Include(y => y.CarImagesEntities);
        _dbSet.Update(entity);
    }

    public void DeleteSaleCarInformation(CarFeaturesEntity entity)
    {
        _dbSet.Include(y => y.CarImagesEntities);
        _dbSet.Remove(entity);
    }
}   