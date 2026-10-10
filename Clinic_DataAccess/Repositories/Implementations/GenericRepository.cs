using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Clinic_DataAccess.Context;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ClinicDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ClinicDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Read
        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

      
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        // Create
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

       
        // Update
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Delete
        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

       
    }
}
