using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetByInvoiceAsync(int invoiceId);
        Task<decimal> GetTotalPaidAsync(int invoiceId);
    }
}
