using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyCarForSale.Core.Repositories;
using MyCarForSale.Core.Services;
using MyCarForSale.Core.UnitOfWorks;
using MyCarForSale.Service.Exceptions;

namespace MyCarForSale.Service.Services;

public class GenericService<T> : IGenericService<T> where T : class
{
    private readonly IGenericRepository<T> _genericRepository;
    protected readonly IUnitOfWork _unitOfWork;


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
        var hasProduct = await _genericRepository.GetByIdAsyncTask(id);

        if (hasProduct == null)
        {
            throw new NotFoundException($"{typeof(T).Name} not found.");
        }

        return hasProduct;
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _genericRepository.Where(expression);
    }

    public async Task<bool> AnyAsyncTask(Expression<Func<T, bool>> expression)
    {
        return await _genericRepository.AnyAsyncTask(expression);
    }

    public async Task<T?> SingleAsyncTask(Expression<Func<T, bool>> expression)
    {
        var hasProduct = await _genericRepository.SingleAsyncTask(expression);

        return hasProduct;
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