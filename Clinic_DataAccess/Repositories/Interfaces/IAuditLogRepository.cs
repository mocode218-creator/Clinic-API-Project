using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    

    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        Task<AuditLog?> GetByLongIdAsync(long id);
        Task<IEnumerable<AuditLog>> GetByRecordAsync(string tableName, long recordId);
        Task<IEnumerable<AuditLog>> GetByUserAsync(int userId);
    }

}
