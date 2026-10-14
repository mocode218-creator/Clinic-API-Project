using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class DoctorScheduleRepository : GenericRepository<DoctorSchedule>, IDoctorScheduleRepository
    {
        public DoctorScheduleRepository(ClinicDbContext context) : base(context) { }

        public async Task<IEnumerable<DoctorSchedule>> GetByDoctorAsync(int doctorId)
        {
            return await _dbSet
                .Include(ds => ds.Doctor).ThenInclude(d => d.Person)
                .Where(ds => ds.DoctorId == doctorId)
                .OrderBy(ds => ds.DayOfWeek)
                .ThenBy(ds => ds.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<DoctorSchedule>> GetByDoctorAndDayAsync(int doctorId, byte dayOfWeek)
        {
            return await _dbSet
       .Include(ds => ds.Doctor).ThenInclude(d => d.Person)
       .Where(ds => ds.DoctorId == doctorId && ds.DayOfWeek == dayOfWeek)
       .OrderBy(ds => ds.StartTime)
       .ToListAsync();
        }

        public async Task<bool> IsOverlappingAsync(int doctorId, byte dayOfWeek, TimeOnly startTime, TimeOnly endTime, int? excludeId = null)
        {
            var query = _dbSet.Where(ds =>
                ds.DoctorId == doctorId &&
                ds.DayOfWeek == dayOfWeek &&
                ds.StartTime < endTime &&
                ds.EndTime > startTime);

            if (excludeId.HasValue)
                query = query.Where(ds => ds.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public override async Task<IEnumerable<DoctorSchedule>> GetAllAsync()
        {
            return await _dbSet
                .Include(ds => ds.Doctor).ThenInclude(d => d.Person)
                .ToListAsync();
        }

        public override async Task<DoctorSchedule?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(ds => ds.Doctor).ThenInclude(d => d.Person)
                .FirstOrDefaultAsync(ds => ds.Id == id);
        }

    }
}
