using System;
using System.Collections.Generic;
using System.Text;
using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Clinic_DataAccess.Repositories.Implementations
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ClinicDbContext context) : base(context) { }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.Name == name && r.DeletedAt == null);
        }

        public override async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _dbSet.Where(r => r.DeletedAt == null).ToListAsync();
        }
    }
}
