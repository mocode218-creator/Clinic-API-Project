using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class PrescriptionItemRepository : GenericRepository<PrescriptionItem>, IPrescriptionItemRepository
    {
        public PrescriptionItemRepository(ClinicDbContext context) : base(context) { }

        public async Task<IEnumerable<PrescriptionItem>> GetByPrescriptionAsync(int prescriptionId)
        {
            return await _dbSet
                .Where(pi => pi.PrescriptionId == prescriptionId)
                .ToListAsync();
        }
    }
}
