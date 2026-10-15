using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.Visits;
using Clinic_BusinessLayer.DTOs.Responses.Visits;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IVisitService
    {
        Task<IEnumerable<VisitResponse>> GetAllAsync();
        Task<VisitResponse> GetByIdAsync(int id);
        Task<VisitResponse> GetByAppointmentAsync(int appointmentId);
        Task<VisitResponse> CreateAsync(CreateVisitRequest request);
        Task<VisitResponse> UpdateAsync(int id, UpdateVisitRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
