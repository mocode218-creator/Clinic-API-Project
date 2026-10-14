using System;
using System.Collections.Generic;
using System.Text;
using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(ClinicDbContext context) : base(context) { }

        public async Task<Specialization?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Name == name && s.DeletedAt == null);
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            var query = _dbSet.Where(s => s.Name == name && s.DeletedAt == null);

            if (excludeId.HasValue)
                query = query.Where(s => s.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public override async Task<IEnumerable<Specialization>> GetAllAsync()
        {
            return await _dbSet
                .Where(s => s.DeletedAt == null)
                .ToListAsync();
        }
    }
}
