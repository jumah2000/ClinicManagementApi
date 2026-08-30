using ClinicManagementAPI.Data;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Repository.Implementations;

public class MedicalRecordRepository(AppDbContext context) : IMedicalRecordRepository
{
        private readonly AppDbContext _context = context;

        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                .ThenInclude(p => p.User)
                .Include(m => m.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                .ThenInclude(p => p.User)
                .Include(m => m.Doctor)
                .ThenInclude(d => d.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetByPatientUserIdAsync(string userId)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                .ThenInclude(p => p.User)
                .Include(m => m.Doctor)
                .ThenInclude(d => d.User)
                .Where(m => m.Patient.User.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetByDoctorUserIdAsync(string userId)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                .ThenInclude(p => p.User)
                .Include(m => m.Doctor)
                .ThenInclude(d => d.User)
                .Where(m => m.Doctor.User.UserId == userId)
                .ToListAsync();
        }

        public async Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord)
        {
            await _context.MedicalRecords.AddAsync(medicalRecord);
            await _context.SaveChangesAsync();

            return medicalRecord;
        }

        public async Task UpdateAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Update(medicalRecord);
            await _context.SaveChangesAsync();
        }
}