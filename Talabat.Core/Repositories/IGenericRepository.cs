using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    IAsyncEnumerable<T> GetAll();
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);
    Task<T> GetEntityWithSpecification(ISpecification<T> specification);
    Task<int> GetCountWithSpecAsync(ISpecification<T> specification);
    Task Add(T entity);
    void Delete(T entity);
    void Update(T entity);
}
