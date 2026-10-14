using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Branches;
using Clinic_BusinessLayer.DTOs.Responses.Branches;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BranchResponse>> GetAllAsync()
        {
            var branches = await _unitOfWork.Branches.GetAllAsync();
            return _mapper.Map<IEnumerable<BranchResponse>>(branches);
        }

        public async Task<BranchResponse?> GetByIdAsync(int id)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(id);
            if (branch == null || branch.DeletedAt != null)
                return null;

            return _mapper.Map<BranchResponse>(branch);
        }

        public async Task<BranchResponse> CreateAsync(CreateBranchRequest request)
        {
            var existing = await _unitOfWork.Branches.GetByNameAsync(request.Name);
            if (existing != null)
                throw new InvalidOperationException($"Branch with name '{request.Name}' already exists");

            var branch = _mapper.Map<Branch>(request);
            await _unitOfWork.Branches.AddAsync(branch);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BranchResponse>(branch);
        }

        public async Task<BranchResponse> UpdateAsync(int id, UpdateBranchRequest request)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(id);
            if (branch == null || branch.DeletedAt != null)
                throw new KeyNotFoundException($"Branch with id {id} not found");

            if (branch.Name != request.Name)
            {
                var existing = await _unitOfWork.Branches.GetByNameAsync(request.Name);
                if (existing != null && existing.Id != id)
                    throw new InvalidOperationException($"Branch with name '{request.Name}' already exists");
            }

            _mapper.Map(request, branch);
            _unitOfWork.Branches.Update(branch);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BranchResponse>(branch);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(id);
            if (branch == null || branch.DeletedAt != null)
                return false;

            branch.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Branches.Update(branch);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
