using ClinicManagementAPI.Data;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Repository.Implementations;

public class OtpRepository(AppDbContext context) : IOtpRepository
{
        private readonly AppDbContext _context = context;

        public async Task<Otp?> GetUnusedOtpAsync(int userId, string code)
        {
            return await _context.Otps
                .Where(x =>
                    x.UserId == userId &&
                    x.Code == code &&
                    !x.IsUsed)
                .FirstOrDefaultAsync();
        }

        public async Task<Otp?> GetLatestUnusedOtpAsync(int userId)
        {
            return await _context.Otps
                .Where(x =>
                    x.UserId == userId &&
                    !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task InvalidatePreviousOtpsAsync(int userId)
        {
            var otps = await _context.Otps
                .Where(x =>
                    x.UserId == userId &&
                    !x.IsUsed)
                .ToListAsync();

            foreach (var otp in otps)
            {
                otp.IsUsed = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Otp> CreateAsync(Otp otp)
        {
            await _context.Otps.AddAsync(otp);
            await _context.SaveChangesAsync();

            return otp;
        }

        public async Task UpdateAsync(Otp otp)
        {
            _context.Otps.Update(otp);
            await _context.SaveChangesAsync();
        }
}