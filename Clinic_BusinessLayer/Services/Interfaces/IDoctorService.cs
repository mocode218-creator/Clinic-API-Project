using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.Doctors;
using Clinic_BusinessLayer.DTOs.Responses.Doctors;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponse>> GetAllAsync();
        Task<DoctorResponse> GetByIdAsync(int id);
        Task<IEnumerable<DoctorResponse>> GetBySpecializationAsync(int specializationId);
        Task<DoctorResponse> CreateAsync(CreateDoctorRequest request);
        Task<DoctorResponse> UpdateAsync(int id, UpdateDoctorRequest request);
        Task<bool> DeleteAsync(int id);
    }
}