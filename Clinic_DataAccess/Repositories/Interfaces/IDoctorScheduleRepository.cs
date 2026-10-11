using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IDoctorScheduleRepository : IGenericRepository<DoctorSchedule>
    {
        Task<IEnumerable<DoctorSchedule>> GetByDoctorAsync(int doctorId);
        
    }
}
