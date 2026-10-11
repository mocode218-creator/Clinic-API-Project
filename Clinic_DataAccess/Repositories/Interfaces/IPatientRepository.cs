using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        
        Task<Patient?> GetByPersonIdAsync(int personId);
        
    }
}
