using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Payments;
using Clinic_BusinessLayer.DTOs.Responses.Payments;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentResponse>> GetAllAsync()
        {
            var payments = await _unitOfWork.Payments.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentResponse>>(payments);
        }

        public async Task<PaymentResponse?> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment == null)
                return null;

            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<IEnumerable<PaymentResponse>> GetByInvoiceAsync(int invoiceId)
        {
            var payments = await _unitOfWork.Payments.GetByInvoiceAsync(invoiceId);
            return _mapper.Map<IEnumerable<PaymentResponse>>(payments);
        }

        public async Task<PaymentResponse> CreateAsync(CreatePaymentRequest request)
        {
            // 1. التحقق من الفاتورة
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(request.InvoiceId);
            if (invoice == null)
                throw new KeyNotFoundException($"Invoice with id {request.InvoiceId} not found");

            // 2. إنشاء الدفع
            var payment = _mapper.Map<Payment>(request);
            await _unitOfWork.Payments.AddAsync(payment);

            // 3. تحديث حالة الفاتورة لو اتدفعت كاملة
            var totalPaid = await _unitOfWork.Payments.GetTotalPaidAsync(request.InvoiceId) + request.Amount;
            if (totalPaid >= invoice.Amount)
            {
                invoice.Status = "Paid";
                _unitOfWork.Invoices.Update(invoice);
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PaymentResponse>(payment);
        }
    }
}
