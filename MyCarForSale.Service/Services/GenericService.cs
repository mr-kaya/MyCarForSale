using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyCarForSale.Core.Repositories;
using MyCarForSale.Core.Services;
using MyCarForSale.Core.UnitOfWorks;

namespace MyCarForSale.Service.Services;

public class GenericService<T> : IGenericService<T> where T : class
{
    private readonly IGenericRepository<T> _genericRepository;
    private readonly IUnitOfWork _unitOfWork;


    public GenericService(IGenericRepository<T> genericRepository, IUnitOfWork unitOfWork)
    {
        _genericRepository = genericRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<T>> GetAllAsyncTask()
    {
        return await _genericRepository.GetAll().ToListAsync();
    }

    public async Task<T> GetByIdAsyncTask(int id)
    {
        return await _genericRepository.GetByIdAsyncTask(id);
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _genericRepository.Where(expression);
    }

    public async Task<bool> AnyAsyncTask(Expression<Func<T, bool>> expression)
    {
        return await _genericRepository.AnyAsyncTask(expression);
    }

    public async Task AddAsyncTask(T entity)
    {
        await _genericRepository.AddAsyncTask(entity);
        await _unitOfWork.CommitAsyncTask();
    }

    public async Task AddRangeAsyncTask(IEnumerable<T> entityEnumerable)
    {
        await _genericRepository.AddRangeAsyncTask(entityEnumerable);
    }

    public async Task UpdateAsyncTask(T entity)
    {
        _genericRepository.Update(entity);
        await _unitOfWork.CommitAsyncTask();
    }

    public async Task DeleteAsyncTask(T entity)
    {
        _genericRepository.Delete(entity);
        await _unitOfWork.CommitAsyncTask();
    }

    public async Task DeleteRangeAsyncTask(IEnumerable<T> entityEnumerable)
    {
        _genericRepository.DeleteRange(entityEnumerable);
        await _unitOfWork.CommitAsyncTask();
    }
}