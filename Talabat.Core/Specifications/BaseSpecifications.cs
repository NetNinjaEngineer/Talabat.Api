using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public abstract class BaseSpecifications<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = [];
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public BaseSpecifications() { }

        public BaseSpecifications(Expression<Func<T, bool>> criteria)
            => Criteria = criteria;

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
            => OrderBy = orderByExpression;

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByExpression)
            => OrderByDescending = orderByExpression;

        protected void ApplyPagination(int take, int skip)
        {
            Take = take;
            Skip = skip;
            IsPaginationEnabled = true;
        }
    }
}
