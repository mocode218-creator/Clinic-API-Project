using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ClinicDbContext context) : base(context) { }

       

        public async Task<Doctor?> GetByLicenseNumberAsync(string licenseNumber)
        {
            return await _dbSet
                .Include(d => d.Person)
                .FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber && d.DeletedAt == null);
        }

        public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(int specializationId)
        {
            return await _dbSet
                .Include(d => d.Person)
                .Where(d => d.SpecializationId == specializationId && d.DeletedAt == null)
                .ToListAsync();
        }

       
    }
}
