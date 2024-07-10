using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);
    Task<T> GetByIdWithSpecification(ISpecification<T> specification);
    Task<int> GetCountWithSpecAsync(ISpecification<T> specification);
}
