using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Repository.Interfaces;

public interface IAuditLogRepository
{
    Task<AuditLog> CreateAsync(AuditLog auditLog);

    Task<IEnumerable<AuditLog>> GetAllAsync();
}