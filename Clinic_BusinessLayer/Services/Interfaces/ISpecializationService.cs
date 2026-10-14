using System;
using System.Collections.Generic;
using System.Text;
using Clinic_BusinessLayer.DTOs.Requests.Specializations;
using Clinic_BusinessLayer.DTOs.Responses.Specializations;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface ISpecializationService
    {
        Task<IEnumerable<SpecializationResponse>> GetAllAsync();
        Task<SpecializationResponse?> GetByIdAsync(int id);
        Task<SpecializationResponse> CreateAsync(CreateSpecializationRequest request);
        Task<SpecializationResponse> UpdateAsync(int id, UpdateSpecializationRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
