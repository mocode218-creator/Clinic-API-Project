using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Responses.Invoices;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceResponse>> GetAllAsync();
        Task<InvoiceResponse> GetByIdAsync(int id);
        Task<InvoiceResponse> GetByVisitAsync(int visitId);
        Task<InvoiceResponse> GenerateAsync(int visitId);
        Task<InvoiceResponse> UpdateStatusAsync(int id, string status);
    }
}