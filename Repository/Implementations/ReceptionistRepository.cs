using ClinicManagementAPI.Data;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Repository.Implementations;

public class ReceptionistRepository(AppDbContext context) : IReceptionistRepository
{
        private readonly AppDbContext _context = context;

        public async Task<Receptionist?> GetByIdAsync(int id)
        {
            return await _context.Receptionists
                .Include(r => r.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Receptionist?> GetByUserIdAsync(int userId)
        {
            return await _context.Receptionists
                .Include(r => r.User)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Receptionist>> GetAllAsync()
        {
            return await _context.Receptionists
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<Receptionist> CreateAsync(Receptionist receptionist)
        {
            await _context.Receptionists.AddAsync(receptionist);
            await _context.SaveChangesAsync();

            return receptionist;
        }

        public async Task UpdateAsync(Receptionist receptionist)
        {
            _context.Receptionists.Update(receptionist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var receptionist = await _context.Receptionists
                .Include(r => r.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (receptionist != null && receptionist.User != null)
            {
                receptionist.User.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
}