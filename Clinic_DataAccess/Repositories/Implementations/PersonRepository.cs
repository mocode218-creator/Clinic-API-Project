using System;
using System.Collections.Generic;
using System.Text;
using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Clinic_DataAccess.Repositories.Implementations
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(ClinicDbContext context) : base(context) { }

        public async Task<Person?> GetByPhoneAsync(string phone)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Phone == phone && p.DeletedAt == null);
        }

        public async Task<Person?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Email == email && p.DeletedAt == null);
        }

       
    }
}
