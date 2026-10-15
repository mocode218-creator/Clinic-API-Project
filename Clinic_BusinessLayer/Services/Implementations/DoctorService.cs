using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Doctors;
using Clinic_BusinessLayer.DTOs.Responses.Doctors;
using Clinic_BusinessLayer.Exceptions;                       // ← 🆕
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorResponse>> GetAllAsync()
        {
            var doctors = await _unitOfWork.Doctors.GetAllWithPersonAsync();
            return _mapper.Map<IEnumerable<DoctorResponse>>(doctors);
        }

        public async Task<DoctorResponse> GetByIdAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetWithPersonAsync(id);
            if (doctor == null || doctor.DeletedAt != null)
                throw new NotFoundException($"Doctor with id {id} not found");

            return _mapper.Map<DoctorResponse>(doctor);
        }

        public async Task<IEnumerable<DoctorResponse>> GetBySpecializationAsync(int specializationId)
        {
            var doctors = await _unitOfWork.Doctors.GetBySpecializationAsync(specializationId);
            return _mapper.Map<IEnumerable<DoctorResponse>>(doctors);
        }

        public async Task<DoctorResponse> CreateAsync(CreateDoctorRequest request)
        {
            var existing = await _unitOfWork.Doctors.GetByLicenseNumberAsync(request.LicenseNumber);
            if (existing != null)
                throw new ConflictException($"Doctor with license '{request.LicenseNumber}' already exists");

            var specialization = await _unitOfWork.Specializations.GetByIdAsync(request.SpecializationId);
            if (specialization == null || specialization.DeletedAt != null)
                throw new NotFoundException($"Specialization with id {request.SpecializationId} not found");

            var person = _mapper.Map<Person>(request);
            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.SaveChangesAsync();

            var doctor = _mapper.Map<Doctor>(request);
            doctor.PersonId = person.Id;
            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            doctor.Person = person;
            doctor.Specialization = specialization;

            return _mapper.Map<DoctorResponse>(doctor);
        }

        public async Task<DoctorResponse> UpdateAsync(int id, UpdateDoctorRequest request)
        {
            var doctor = await _unitOfWork.Doctors.GetWithPersonAsync(id);
            if (doctor == null || doctor.DeletedAt != null)
                throw new NotFoundException($"Doctor with id {id} not found");

            if (doctor.LicenseNumber != request.LicenseNumber)
            {
                var existing = await _unitOfWork.Doctors.GetByLicenseNumberAsync(request.LicenseNumber);
                if (existing != null && existing.Id != id)
                    throw new ConflictException($"Doctor with license '{request.LicenseNumber}' already exists");
            }

            _mapper.Map(request, doctor.Person);
            _unitOfWork.Persons.Update(doctor.Person);

            _mapper.Map(request, doctor);
            _unitOfWork.Doctors.Update(doctor);

            await _unitOfWork.SaveChangesAsync();

            var updated = await _unitOfWork.Doctors.GetWithPersonAsync(id);
            return _mapper.Map<DoctorResponse>(updated!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null || doctor.DeletedAt != null)
                throw new NotFoundException($"Doctor with id {id} not found");

            doctor.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}