using ClinicManagementAPI.Data;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Repository.Implementations;

public class AdminRepository(AppDbContext context) : IAdminRepository
{
        private readonly AppDbContext _context = context;

        public async Task<Admin?> GetByIdAsync(int id)
        {
            return await _context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Admin?> GetByUserIdAsync(int userId)
        {
            return await _context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _context.Admins
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<Admin> CreateAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();

            return admin;
        }

        public async Task UpdateAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var admin = await _context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (admin != null && admin.User != null)
            {
                admin.User.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
}