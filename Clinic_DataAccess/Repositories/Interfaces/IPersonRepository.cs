using System;
using System.Collections.Generic;
using System.Text;
using Clinic_DataAccess.Entities;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<Person?> GetByPhoneAsync(string phone);
        Task<Person?> GetByEmailAsync(string email);
        
    }
}
