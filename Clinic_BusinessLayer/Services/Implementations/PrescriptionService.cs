using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Prescriptions;
using Clinic_BusinessLayer.DTOs.Responses.Prescriptions;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrescriptionResponse>> GetAllAsync()
        {
            var prescriptions = await _unitOfWork.Prescriptions.GetAllAsync();
            return _mapper.Map<IEnumerable<PrescriptionResponse>>(prescriptions);
        }

        public async Task<PrescriptionResponse?> GetByIdAsync(int id)
        {
            var prescription = await _unitOfWork.Prescriptions.GetWithItemsAsync(id);
            if (prescription == null)
                return null;

            return _mapper.Map<PrescriptionResponse>(prescription);
        }

        public async Task<IEnumerable<PrescriptionResponse>> GetByVisitAsync(int visitId)
        {
            var prescriptions = await _unitOfWork.Prescriptions.GetByVisitAsync(visitId);
            return _mapper.Map<IEnumerable<PrescriptionResponse>>(prescriptions);
        }

        public async Task<PrescriptionResponse> CreateAsync(CreatePrescriptionRequest request)
        {
            // 1. التحقق من الـ Visit
            var visit = await _unitOfWork.Visits.GetByIdAsync(request.VisitId);
            if (visit == null)
                throw new KeyNotFoundException($"Visit with id {request.VisitId} not found");

            // 2. إنشاء الـ Prescription
            var prescription = _mapper.Map<Prescription>(request);
            await _unitOfWork.Prescriptions.AddAsync(prescription);
            await _unitOfWork.SaveChangesAsync();

            // 3. إضافة الـ Items
            foreach (var itemRequest in request.Items)
            {
                var item = _mapper.Map<PrescriptionItem>(itemRequest);
                item.PrescriptionId = prescription.Id;
                await _unitOfWork.PrescriptionItems.AddAsync(item);
            }

            await _unitOfWork.SaveChangesAsync();

            // 4. تحميل الـ Items
            prescription.PrescriptionItems = (await _unitOfWork.Prescriptions.GetWithItemsAsync(prescription.Id))!.PrescriptionItems;

            return _mapper.Map<PrescriptionResponse>(prescription);
        }

        public async Task<PrescriptionResponse> UpdateAsync(int id, UpdatePrescriptionRequest request)
        {
            var prescription = await _unitOfWork.Prescriptions.GetWithItemsAsync(id);
            if (prescription == null)
                throw new KeyNotFoundException($"Prescription with id {id} not found");

            _mapper.Map(request, prescription);
            _unitOfWork.Prescriptions.Update(prescription);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PrescriptionResponse>(prescription);
        }
    }
}
