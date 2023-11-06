using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyCarForSale.Core.Repositories;

namespace MyCarForSale.Repository.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }
    
    public IQueryable<T> GetAll()
    {
        return _dbSet.AsNoTracking().AsQueryable();
    }

    public async Task<T> GetByIdAsyncTask(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }

    public async Task<bool> AnyAsyncTask(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public async Task AddAsyncTask(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsyncTask(IEnumerable<T> entityEnumerable)
    {
        await _dbSet.AddRangeAsync(entityEnumerable);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entityEnumerable)
    {
        _dbSet.RemoveRange(entityEnumerable);
    }
}