using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class AuditLogService(IAuditLogRepository auditLogRepository) : IAuditLogService
{
        private readonly IAuditLogRepository _auditLogRepository = auditLogRepository;

        public async Task CreateAsync(string action, string status, string description, int? userId = null)
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                Status = status,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            await _auditLogRepository.CreateAsync(auditLog);
        }
}