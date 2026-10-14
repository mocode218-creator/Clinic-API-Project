using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.DoctorSchedules;
using Clinic_BusinessLayer.DTOs.Responses.DoctorSchedules;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IDoctorScheduleService
    {
        Task<IEnumerable<DoctorScheduleResponse>> GetAllAsync();
        Task<DoctorScheduleResponse?> GetByIdAsync(int id);
        Task<IEnumerable<DoctorScheduleResponse>> GetByDoctorAsync(int doctorId);
        Task<DoctorScheduleResponse> CreateAsync(CreateDoctorScheduleRequest request);
        Task<DoctorScheduleResponse> UpdateAsync(int id, UpdateDoctorScheduleRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
