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

        public async Task<IEnumerable<Appointment>> GetByPatientAsync(int patientId)
        {
            return await _dbSet
                .Include(a => a.Patient).ThenInclude(p => p.Person)   
                .Include(a => a.Doctor).ThenInclude(d => d.Person)
                .Include(a => a.Branch)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);

            return await _dbSet
                .Include(a => a.Patient).ThenInclude(p => p.Person)
                .Include(a => a.Doctor).ThenInclude(d => d.Person)
                .Include(a => a.Branch)
                .Where(a => a.DoctorId == doctorId &&
                            a.StartDate >= dayStart &&
                            a.StartDate < dayEnd)
                .OrderBy(a => a.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByBranchAndDateAsync(int branchId, DateTime date)
        {
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);

            return await _dbSet
                .Include(a => a.Patient).ThenInclude(p => p.Person)
                .Include(a => a.Doctor).ThenInclude(d => d.Person)
                .Include(a => a.Branch)
                .Where(a => a.BranchId == branchId &&
                            a.StartDate >= dayStart &&
                            a.StartDate < dayEnd)
                .OrderBy(a => a.StartDate)
                .ToListAsync();
        }

        public async Task<bool> IsSlotBookedAsync(int doctorId, DateTime startDate, int? excludeId = null)
        {
            var query = _dbSet.Where(a =>
                a.DoctorId == doctorId &&
                a.StartDate == startDate &&
                a.Status != "Cancelled");

            if (excludeId.HasValue)
                query = query.Where(a => a.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public override async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _dbSet
                .Include(a => a.Patient).ThenInclude(p => p.Person)
                .Include(a => a.Doctor).ThenInclude(d => d.Person)
                .Include(a => a.Branch)
                .ToListAsync();
        }


    }
}
