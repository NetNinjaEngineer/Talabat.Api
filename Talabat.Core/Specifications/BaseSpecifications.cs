using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }

        public List<Expression<Func<T, object>>> Includes { get; set; } = [];

        public BaseSpecifications() { }

        public BaseSpecifications(Expression<Func<T, bool>> criteria)
            => Criteria = criteria;


    }
}
