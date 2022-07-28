using System.Collections;
using SetValues.Server.Contexts;

namespace SetValues.Server.Repositories;

public sealed class UnitOfWork<TId> : IUnitOfWork<TId>
{
    private readonly ApplicationContext _dbContext;
    private Hashtable? _repositories;
    private bool _disposed;

    public UnitOfWork(ApplicationContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IRepositoryAsync<TEntity, TId> Repository<TEntity>() where TEntity : AuditableEntity<TId>
    {
        _repositories ??= new Hashtable();

        var type = typeof(TEntity).Name;

        if (_repositories.ContainsKey(type)) return (IRepositoryAsync<TEntity, TId>) _repositories[type];
        var repositoryType = typeof(RepositoryAsync<,>);

        var repositoryInstance = Activator.CreateInstance(
            repositoryType.MakeGenericType(
                typeof(TEntity),
                typeof(TId)),
            _dbContext);

        _repositories.Add(
            type,
            repositoryInstance);

        return (IRepositoryAsync<TEntity, TId>) _repositories[type]!;
    }

    public async Task<int> Commit(CancellationToken cancellationToken) => await _dbContext.SaveChangesAsync(cancellationToken);

    public Task Rollback()
    {
        _dbContext.ChangeTracker.Entries()
            .ToList()
            .ForEach(x => x.Reload());
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _dbContext.Dispose();

        _disposed = true;
    }
}
    
public interface IUnitOfWork<TId> : IDisposable
{
    IRepositoryAsync<T, TId> Repository<T>() where T : AuditableEntity<TId>;
    Task<int> Commit(CancellationToken cancellationToken);
    Task Rollback();
}