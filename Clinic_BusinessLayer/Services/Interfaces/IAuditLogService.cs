using System;
using System.Collections.Generic;
using System.Text;

using Clinic_BusinessLayer.DTOs.Responses.AuditLogs;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task<IEnumerable<AuditLogResponse>> GetAllAsync();
        Task<AuditLogResponse?> GetByIdAsync(long id);
        Task<IEnumerable<AuditLogResponse>> GetByRecordAsync(string tableName, long recordId);
        Task<IEnumerable<AuditLogResponse>> GetByUserAsync(int userId);
        Task LogAsync(int? userId, string tableName, long recordId, string field, string? oldValue, string? newValue);
    }
}