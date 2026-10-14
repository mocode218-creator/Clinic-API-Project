using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.Branches;
using Clinic_BusinessLayer.DTOs.Responses.Branches;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchResponse>> GetAllAsync();
        Task<BranchResponse?> GetByIdAsync(int id);
        Task<BranchResponse> CreateAsync(CreateBranchRequest request);
        Task<BranchResponse> UpdateAsync(int id, UpdateBranchRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
