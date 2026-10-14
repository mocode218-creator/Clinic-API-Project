using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(ClinicDbContext context) : base(context) { }

        public async Task<Patient?> GetWithPersonAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Person)
                .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);
        }

        public async Task<Patient?> GetByPersonIdAsync(int personId)
        {
            return await _dbSet
                .Include(p => p.Person)
                .FirstOrDefaultAsync(p => p.PersonId == personId && p.DeletedAt == null);
        }

        public async Task<IEnumerable<Patient>> SearchByNameOrPhoneAsync(string searchTerm)
        {
            return await _dbSet
                .Include(p => p.Person)
                .Where(p => p.DeletedAt == null &&
                    (p.Person.FirstName.Contains(searchTerm) ||
                     p.Person.LastName.Contains(searchTerm) ||
                     (p.Person.Phone != null && p.Person.Phone.Contains(searchTerm))))
                .ToListAsync();
        }

        public override async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Person)
                .Where(p => p.DeletedAt == null)
                .ToListAsync();
        }
    }
}
