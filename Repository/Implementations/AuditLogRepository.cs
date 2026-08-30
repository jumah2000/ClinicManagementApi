using ClinicManagementAPI.Data;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Repository.Implementations;

public class AuditLogRepository(AppDbContext context) : IAuditLogRepository
{
        private readonly AppDbContext _context = context;

        public async Task<AuditLog> CreateAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();

            return auditLog;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .ToListAsync();
        }
}