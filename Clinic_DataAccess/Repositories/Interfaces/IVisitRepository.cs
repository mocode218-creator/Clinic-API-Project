using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IVisitRepository : IGenericRepository<Visit>
    {
        Task<Visit?> GetByAppointmentAsync(int appointmentId);
        Task<Visit?> GetWithDetailsAsync(int id);
    }
}
