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

        

        public override async Task<IEnumerable<Specialization>> GetAllAsync()
        {
            return await _dbSet
                .Where(s => s.DeletedAt == null)
                .ToListAsync();
        }
    }
}
