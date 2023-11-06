using MyCarForSale.Core.UnitOfWorks;

namespace MyCarForSale.Repository.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
     
    public async Task CommitAsyncTask()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Commit()
    {
        _dbContext.SaveChanges();
    }
}