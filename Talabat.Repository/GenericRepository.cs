﻿using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T>(StoreContext storeContext)
        : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _storeContext = storeContext;

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _storeContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _storeContext.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(
            ISpecification<T> specification)
        {
            var result = await ApplySpecification(specification).ToListAsync();
            return result;
        }


        public async Task<T> GetEntityWithSpecification(ISpecification<T> specification)
            => await ApplySpecification(specification).FirstOrDefaultAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
            => SpecificationEvaluator<T>.BuildQuery(_storeContext.Set<T>(), specification);

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        public async Task Add(T entity)
            => await _storeContext.Set<T>().AddAsync(entity);

        public void Delete(T entity) => _storeContext.Remove(entity);

        public void Update(T entity) => _storeContext.Update(entity);
    }
}
