using AutoMapper;
using Clinic_BusinessLayer.DTOs.Responses.Invoices;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InvoiceResponse>> GetAllAsync()
        {
            var invoices = await _unitOfWork.Invoices.GetAllAsync();
            return _mapper.Map<IEnumerable<InvoiceResponse>>(invoices);
        }

        public async Task<InvoiceResponse?> GetByIdAsync(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetWithPaymentsAsync(id);
            if (invoice == null)
                return null;

            return _mapper.Map<InvoiceResponse>(invoice);
        }

        public async Task<InvoiceResponse?> GetByVisitAsync(int visitId)
        {
            var invoice = await _unitOfWork.Invoices.GetByVisitAsync(visitId);
            if (invoice == null)
                return null;

            return _mapper.Map<InvoiceResponse>(invoice);
        }

        public async Task<InvoiceResponse> GenerateAsync(int visitId)
        {
            // 1. التحقق من الـ Visit
            var visit = await _unitOfWork.Visits.GetByIdAsync(visitId);
            if (visit == null)
                throw new KeyNotFoundException($"Visit with id {visitId} not found");

            // 2. التحقق إن مش فيه فاتورة أصلًا
            var existing = await _unitOfWork.Invoices.GetByVisitAsync(visitId);
            if (existing != null)
                throw new InvalidOperationException("This visit already has an invoice");

            // 3. إنشاء الفاتورة
            var invoice = new Invoice
            {
                VisitId = visitId,
                Amount = 500m,   // ← سعر ثابت مؤقت
                Status = "Unpaid"
            };

            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InvoiceResponse>(invoice);
        }

        public async Task<InvoiceResponse> UpdateStatusAsync(int id, string status)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(id);
            if (invoice == null)
                throw new KeyNotFoundException($"Invoice with id {id} not found");

            if (status != "Paid" && status != "Unpaid")
                throw new InvalidOperationException("Status must be 'Paid' or 'Unpaid'");

            invoice.Status = status;
            _unitOfWork.Invoices.Update(invoice);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<InvoiceResponse>(invoice);
        }
    }
}
