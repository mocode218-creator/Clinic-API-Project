using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.Patients;
using Clinic_BusinessLayer.DTOs.Responses.Patients;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponse>> GetAllAsync();
        Task<PatientResponse> GetByIdAsync(int id);
        Task<IEnumerable<PatientResponse>> SearchAsync(string searchTerm);
        Task<PatientResponse> CreateAsync(CreatePatientRequest request);
        Task<PatientResponse> UpdateAsync(int id, UpdatePatientRequest request);
        Task<bool> DeleteAsync(int id);
    }
}