using ClinicManagementAPI.Data;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Repository.Implementations;

public class UserRepository(AppDbContext context) : IUserRepository
{
        private readonly AppDbContext _context = context;

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByUserIdAsync(string userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<User>> GetByUserTypeAsync(string userType)
        {
            return await _context.Users
                .Where(x => x.UserType == userType)
                .ToListAsync();
        }

        public async Task<User?> GetLatestUserByTypeAsync(string userType)
        {
            return await _context.Users
                .Where(x => x.UserType == userType)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
}