using ClinicManagementAPI.Data;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Repository.Implementations;

public class DoctorRepositor(AppDbContext context) : IDoctorRepository
{
        private readonly AppDbContext _context = context;

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Doctor?> GetByUserIdAsync(int userId)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.User)
                .ToListAsync();
        }

        public async Task<Doctor> CreateAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();

            return doctor;
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (doctor != null && doctor.User != null)
            {
                doctor.User.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
}