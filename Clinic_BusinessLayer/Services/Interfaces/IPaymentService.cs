using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.Payments;
using Clinic_BusinessLayer.DTOs.Responses.Payments;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponse>> GetAllAsync();
        Task<PaymentResponse?> GetByIdAsync(int id);
        Task<IEnumerable<PaymentResponse>> GetByInvoiceAsync(int invoiceId);
        Task<PaymentResponse> CreateAsync(CreatePaymentRequest request);
    }
}
