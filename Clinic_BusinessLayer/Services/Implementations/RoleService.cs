using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Roles;
using Clinic_BusinessLayer.DTOs.Responses.Roles;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;


namespace Clinic_BusinessLayer.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleResponse>> GetAllAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleResponse>>(roles);
        }

        public async Task<RoleResponse?> GetByIdAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null || role.DeletedAt != null)
                return null;

            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse> CreateAsync(CreateRoleRequest request)
        {
            var existing = await _unitOfWork.Roles.GetByNameAsync(request.Name);
            if (existing != null)
                throw new InvalidOperationException($"Role with name '{request.Name}' already exists");

            var role = _mapper.Map<Role>(request);
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse> UpdateAsync(int id, UpdateRoleRequest request)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null || role.DeletedAt != null)
                throw new KeyNotFoundException($"Role with id {id} not found");

            if (role.Name != request.Name)
            {
                var existing = await _unitOfWork.Roles.GetByNameAsync(request.Name);
                if (existing != null && existing.Id != id)
                    throw new InvalidOperationException($"Role with name '{request.Name}' already exists");
            }

            _mapper.Map(request, role);
            _unitOfWork.Roles.Update(role);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null || role.DeletedAt != null)
                return false;

            role.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Roles.Update(role);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
