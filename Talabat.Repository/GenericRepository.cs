﻿using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _storeContext;

        public GenericRepository(StoreContext storeContext) => _storeContext = storeContext;

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _storeContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _storeContext.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAllWithSpecificationAsync(ISpecification<T> specification)
            => await ApplySpecification(specification).ToListAsync();


        public async Task<T> GetByIdWithSpecification(ISpecification<T> specification)
            => await ApplySpecification(specification).FirstOrDefaultAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
            => SpecificationEvaluator<T>.BuildQuery(_storeContext.Set<T>(), specification);
    }
}
