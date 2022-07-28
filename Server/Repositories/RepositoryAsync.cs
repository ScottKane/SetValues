using Microsoft.EntityFrameworkCore;
using SetValues.Server.Contexts;

namespace SetValues.Server.Repositories;

public interface IRepositoryAsync<T, in TId> where T : class, IEntity<TId>
{
    IQueryable<T> Entities { get; }
    Task<T> GetByIdAsync(TId id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
    
public class RepositoryAsync<T, TId> : IRepositoryAsync<T, TId> where T : AuditableEntity<TId>
{
    private readonly ApplicationContext _dbContext;

    public RepositoryAsync(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> Entities => _dbContext.Set<T>();

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
            
        return Task.CompletedTask;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>()
            .ToListAsync();
    }

    public async Task<T> GetByIdAsync(TId id)
    {
        return (await _dbContext.Set<T>().FindAsync(id))!;
    }

    public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
    {
        return await _dbContext.Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        var selected = await _dbContext.Set<T>().FindAsync(entity.Id);
        if (selected is not null)
            _dbContext.Entry(selected).CurrentValues.SetValues(entity);
    }
}