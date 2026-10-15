using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.Appointments;
using Clinic_BusinessLayer.DTOs.Responses.Appointments;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentResponse>> GetAllAsync();
        Task<AppointmentResponse> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentResponse>> GetByPatientAsync(int patientId);
        Task<IEnumerable<AppointmentResponse>> GetByDoctorAndDateAsync(int doctorId, DateTime date);
        Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request, int createdByUserId);
        Task<AppointmentResponse> UpdateAsync(int id, UpdateAppointmentRequest request);
        Task<AppointmentResponse> CancelAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}