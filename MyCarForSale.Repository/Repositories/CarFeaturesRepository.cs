﻿using Microsoft.EntityFrameworkCore;
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
        //return await _dbContext.FeaturesBaseEntities.Include(x => x.MainClassificationEntityId).ToListAsync();
        return await _dbContext.FeaturesBaseEntities.Include(x => x.MainClassificationEntity).ToListAsync();
    }
}