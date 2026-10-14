using AutoMapper;
using Clinic_BusinessLayer.DTOs.Responses.AuditLogs;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuditLogResponse>> GetAllAsync()
        {
            var logs = await _unitOfWork.AuditLogs.GetAllAsync();
            return _mapper.Map<IEnumerable<AuditLogResponse>>(logs);
        }

        public async Task<AuditLogResponse?> GetByIdAsync(long id)
        {
            var log = await _unitOfWork.AuditLogs.GetByLongIdAsync(id);
            if (log == null) return null;
            return _mapper.Map<AuditLogResponse>(log);
        }

        public async Task<IEnumerable<AuditLogResponse>> GetByRecordAsync(string tableName, long recordId)
        {
            var logs = await _unitOfWork.AuditLogs.GetByRecordAsync(tableName, recordId);
            return _mapper.Map<IEnumerable<AuditLogResponse>>(logs);
        }

        public async Task<IEnumerable<AuditLogResponse>> GetByUserAsync(int userId)
        {
            var logs = await _unitOfWork.AuditLogs.GetByUserAsync(userId);
            return _mapper.Map<IEnumerable<AuditLogResponse>>(logs);
        }

        public async Task LogAsync(int? userId, string tableName, long recordId, string field, string? oldValue, string? newValue)
        {
            var log = new AuditLog
            {
                UserId = userId,
                TableName = tableName,
                RecordId = recordId,
                Field = field,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedAt = DateTime.UtcNow
            };

            await _unitOfWork.AuditLogs.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
