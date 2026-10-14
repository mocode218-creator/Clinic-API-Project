using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(ClinicDbContext context) : base(context) { }

        public async Task<IEnumerable<Prescription>> GetByVisitAsync(int visitId)
        {
            return await _dbSet
                .Include(p => p.PrescriptionItems)
                .Where(p => p.VisitId == visitId)
                .ToListAsync();
        }

        public async Task<Prescription?> GetWithItemsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.PrescriptionItems)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.PrescriptionItems)
                .ToListAsync();
        }

    }
}
