using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Read
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        // Create
        Task AddAsync(T entity);
       

        // Update
        void Update(T entity);

        // Delete
        void Remove(T entity);
        
    }
}
