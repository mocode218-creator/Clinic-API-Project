using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.Prescriptions;
using Clinic_BusinessLayer.DTOs.Responses.Prescriptions;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionResponse>> GetAllAsync();
        Task<PrescriptionResponse?> GetByIdAsync(int id);
        Task<IEnumerable<PrescriptionResponse>> GetByVisitAsync(int visitId);
        Task<PrescriptionResponse> CreateAsync(CreatePrescriptionRequest request);
        Task<PrescriptionResponse> UpdateAsync(int id, UpdatePrescriptionRequest request);
    }
}