using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor?> GetWithPersonAsync(int id);
        Task<IEnumerable<Doctor>> GetAllWithPersonAsync();
        Task<Doctor?> GetByPersonIdAsync(int personId);
        Task<Doctor?> GetByLicenseNumberAsync(string licenseNumber);
        Task<IEnumerable<Doctor>> GetBySpecializationAsync(int specializationId);
       
    }
}
