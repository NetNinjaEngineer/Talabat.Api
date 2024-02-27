using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> BuildQuery(IQueryable<T> source, ISpecification<T> specification)
        {
            var query = source.AsQueryable();

            if (specification.Criteria is not null)
                query = query.Where(specification.Criteria);

            query = specification.Includes.Aggregate(query, (currecntQuery, includeExpression)
                => currecntQuery.Include(includeExpression));

            return query;

        }
    }
}
