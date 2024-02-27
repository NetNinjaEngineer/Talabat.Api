using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);

        Task<T> GetByIdWithSpecification(ISpecification<T> specification);

    }
}
