using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.Roles;
using Clinic_BusinessLayer.DTOs.Responses.Roles;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetAllAsync();
        Task<RoleResponse?> GetByIdAsync(int id);
        Task<RoleResponse> CreateAsync(CreateRoleRequest request);
        Task<RoleResponse> UpdateAsync(int id, UpdateRoleRequest request);
        Task<bool> DeleteAsync(int id);
    }
}