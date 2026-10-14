using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IDoctorScheduleRepository : IGenericRepository<DoctorSchedule>
    {
       
        Task<IEnumerable<DoctorSchedule>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<DoctorSchedule>> GetByDoctorAndDayAsync(int doctorId, byte dayOfWeek);
        Task<bool> IsOverlappingAsync(int doctorId, byte dayOfWeek, TimeOnly startTime, TimeOnly endTime, int? excludeId = null);

    }
}
