using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Patients;
using Clinic_BusinessLayer.DTOs.Responses.Patients;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;


namespace Clinic_BusinessLayer.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientResponse>> GetAllAsync()
        {
            var patients = await _unitOfWork.Patients.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientResponse>>(patients);
        }

        public async Task<PatientResponse?> GetByIdAsync(int id)
        {
            var patient = await _unitOfWork.Patients.GetWithPersonAsync(id);
            if (patient == null || patient.DeletedAt != null)
                return null;

            return _mapper.Map<PatientResponse>(patient);
        }

        public async Task<IEnumerable<PatientResponse>> SearchAsync(string searchTerm)
        {
            var patients = await _unitOfWork.Patients.SearchByNameOrPhoneAsync(searchTerm);
            return _mapper.Map<IEnumerable<PatientResponse>>(patients);
        }

        public async Task<PatientResponse> CreateAsync(CreatePatientRequest request)
        {
            // 1. إنشاء Person
            var person = _mapper.Map<Person>(request);
            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.SaveChangesAsync();

            // 2. إنشاء Patient وربطه بالـ Person
            var patient = _mapper.Map<Patient>(request);
            patient.PersonId = person.Id;
            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();

            // 3. تحميل الـ Person
            patient.Person = person;

            return _mapper.Map<PatientResponse>(patient);
        }

        public async Task<PatientResponse> UpdateAsync(int id, UpdatePatientRequest request)
        {
            var patient = await _unitOfWork.Patients.GetWithPersonAsync(id);
            if (patient == null || patient.DeletedAt != null)
                throw new KeyNotFoundException($"Patient with id {id} not found");

            // تعديل الـ Person
            _mapper.Map(request, patient.Person);
            _unitOfWork.Persons.Update(patient.Person);

            // تعديل الـ Patient
            _mapper.Map(request, patient);
            _unitOfWork.Patients.Update(patient);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PatientResponse>(patient);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null || patient.DeletedAt != null)
                return false;

            patient.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
