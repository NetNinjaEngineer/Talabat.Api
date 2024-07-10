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

            if (specification.Includes is not null)
                query = specification.Includes.Aggregate(query, (currecntQuery, includeExpression)
                    => currecntQuery.Include(includeExpression));

            if (specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);

            if (specification.OrderByDescending is not null)
                query = query.OrderByDescending(specification.OrderByDescending);

            return query;

        }
    }
}
