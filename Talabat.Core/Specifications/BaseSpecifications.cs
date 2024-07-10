using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = [];
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        public BaseSpecifications() { }

        public BaseSpecifications(Expression<Func<T, bool>> criteria)
            => Criteria = criteria;

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
            => OrderBy = orderByExpression;

        public void AddOrderByDescending(Expression<Func<T, object>> orderByExpression)
            => OrderByDescending = orderByExpression;

    }
}
