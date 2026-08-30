using ClinicManagementAPI.Data;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Repository.Implementations;

public class PatientRepository(AppDbContext context) : IPatientRepository
{
        private readonly AppDbContext _context = context;

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.Include(p => p.User).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            return await _context.Patients
                .Include(p => p.User)  
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.User).ToListAsync();
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.User)  
                .FirstOrDefaultAsync(x => x.Id == id);

            if (patient != null && patient.User != null)
            {
                patient.User.IsActive = false; 
                await _context.SaveChangesAsync();
            }
        }
}