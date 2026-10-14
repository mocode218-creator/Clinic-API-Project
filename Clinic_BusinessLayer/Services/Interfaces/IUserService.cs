using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Requests.Users;
using Clinic_BusinessLayer.DTOs.Responses.Users;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(int id);
        Task<UserResponse> CreateAsync(CreateUserRequest request);
        Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request);
        Task<bool> DeleteAsync(int id);
    }
}