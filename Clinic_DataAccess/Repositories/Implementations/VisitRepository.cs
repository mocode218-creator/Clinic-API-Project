using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class VisitRepository : GenericRepository<Visit>, IVisitRepository
    {
        public VisitRepository(ClinicDbContext context) : base(context) { }

        public async Task<Visit?> GetByAppointmentAsync(int appointmentId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(v => v.AppointmentId == appointmentId);
        }

        public async Task<Visit?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(v => v.Appointment)
                    .ThenInclude(a => a.Patient).ThenInclude(p => p.Person)
                .Include(v => v.Appointment)
                    .ThenInclude(a => a.Doctor).ThenInclude(d => d.Person)
                .Include(v => v.Prescriptions)
                    .ThenInclude(p => p.PrescriptionItems)
                .Include(v => v.Invoices)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
