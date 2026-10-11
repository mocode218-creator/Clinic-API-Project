using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface ISpecializationRepository : IGenericRepository<Specialization>
    {
        Task<Specialization?> GetByNameAsync(string name);
        
    }
}
