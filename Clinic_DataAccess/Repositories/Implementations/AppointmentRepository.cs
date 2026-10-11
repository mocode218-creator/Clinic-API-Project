using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ClinicDbContext context) : base(context) { }

        public async Task<Appointment?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Patient).ThenInclude(p => p.Person)
                .Include(a => a.Doctor).ThenInclude(d => d.Person)
                .Include(a => a.Branch)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

       
    }
}
