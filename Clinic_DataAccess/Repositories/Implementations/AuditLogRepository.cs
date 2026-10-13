using Clinic_DataAccess.Context;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_DataAccess.Repositories.Implementations
{
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ClinicDbContext context) : base(context) { }

        public async Task<IEnumerable<AuditLog>> GetByRecordAsync(string tableName, long recordId)
        {
            return await _dbSet
                .Where(a => a.TableName == tableName && a.RecordId == recordId)
                .OrderByDescending(a => a.ChangedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(int userId)
        {
            return await _dbSet
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.ChangedAt)
                .ToListAsync();
        }
    }
}
