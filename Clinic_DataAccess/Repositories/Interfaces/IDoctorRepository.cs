using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
       
        Task<Doctor?> GetByLicenseNumberAsync(string licenseNumber);
        Task<IEnumerable<Doctor>> GetBySpecializationAsync(int specializationId);
       
    }
}
