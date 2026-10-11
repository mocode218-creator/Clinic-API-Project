using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ClinicDbContext context) : base(context) { }

        public async Task<Invoice?> GetByVisitAsync(int visitId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(i => i.VisitId == visitId);
        }

        public async Task<Invoice?> GetWithPaymentsAsync(int id)
        {
            return await _dbSet
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
