using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IPrescriptionItemRepository : IGenericRepository<PrescriptionItem>
    {
        Task<IEnumerable<PrescriptionItem>> GetByPrescriptionAsync(int prescriptionId);
    }
}
