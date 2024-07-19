using System.Collections;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository;
public class UnitOfWork : IUnitOfWork
{
    private readonly StoreContext _storeContext;
    private Hashtable _repositories;

    public UnitOfWork(StoreContext storeContext)
    {
        _storeContext = storeContext;
        _repositories = [];
    }

    public async Task<int> CompleteAsync()
        => await _storeContext.SaveChangesAsync();

    public async ValueTask DisposeAsync()
        => await _storeContext.DisposeAsync();

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity).Name;
        if (!_repositories.ContainsKey(type))
        {
            var Repository = new GenericRepository<TEntity>(_storeContext);
            _repositories.Add(type, Repository);
        }

        return (IGenericRepository<TEntity>)_repositories[type]!;
    }
}
