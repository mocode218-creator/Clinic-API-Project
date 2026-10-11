using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ClinicDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetByInvoiceAsync(int invoiceId)
        {
            return await _dbSet
                .Where(p => p.InvoiceId == invoiceId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaidAsync(int invoiceId)
        {
            return await _dbSet
                .Where(p => p.InvoiceId == invoiceId)
                .SumAsync(p => p.Amount);
        }
    }
}
