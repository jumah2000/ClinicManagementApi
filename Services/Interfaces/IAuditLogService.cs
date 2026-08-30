namespace ClinicManagementAPI.Services.Interfaces;

public interface IAuditLogService
{
    Task CreateAsync(string action, string status, string description, int? userId = null);
}