using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Specializations;
using Clinic_BusinessLayer.DTOs.Responses.Specializations;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;


namespace Clinic_BusinessLayer.Services.Implementations
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecializationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SpecializationResponse>> GetAllAsync()
        {
            var specializations = await _unitOfWork.Specializations.GetAllAsync();
            return _mapper.Map<IEnumerable<SpecializationResponse>>(specializations);
        }

        public async Task<SpecializationResponse?> GetByIdAsync(int id)
        {
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(id);
            if (specialization == null || specialization.DeletedAt != null)
                return null;

            return _mapper.Map<SpecializationResponse>(specialization);
        }

        public async Task<SpecializationResponse> CreateAsync(CreateSpecializationRequest request)
        {
            var exists = await _unitOfWork.Specializations.IsNameExistsAsync(request.Name);
            if (exists)
                throw new InvalidOperationException($"Specialization with name '{request.Name}' already exists");

            var specialization = _mapper.Map<Specialization>(request);
            await _unitOfWork.Specializations.AddAsync(specialization);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SpecializationResponse>(specialization);
        }

        public async Task<SpecializationResponse> UpdateAsync(int id, UpdateSpecializationRequest request)
        {
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(id);
            if (specialization == null || specialization.DeletedAt != null)
                throw new KeyNotFoundException($"Specialization with id {id} not found");

            if (specialization.Name != request.Name)
            {
                var exists = await _unitOfWork.Specializations.IsNameExistsAsync(request.Name, id);
                if (exists)
                    throw new InvalidOperationException($"Specialization with name '{request.Name}' already exists");
            }

            _mapper.Map(request, specialization);
            _unitOfWork.Specializations.Update(specialization);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SpecializationResponse>(specialization);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(id);
            if (specialization == null || specialization.DeletedAt != null)
                return false;

            specialization.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Specializations.Update(specialization);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

    }
}
