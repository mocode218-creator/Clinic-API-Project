using System;
using System.Collections.Generic;
using System.Text;
using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Clinic_DataAccess.Repositories.Implementations
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(ClinicDbContext context) : base(context) { }

        public async Task<Branch?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.Name == name && b.DeletedAt == null);
        }

        public override async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _dbSet.Where(b => b.DeletedAt == null).ToListAsync();
        }
    }
}
