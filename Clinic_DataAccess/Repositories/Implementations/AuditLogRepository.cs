using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetByRecordAsync(string tableName, long recordId);
        Task<IEnumerable<AuditLog>> GetByUserAsync(int userId);
    }
}
