using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Repository.Interfaces;

public interface IOtpRepository
{
    Task<Otp?> GetUnusedOtpAsync(int userId, string code);
    
    Task<Otp?> GetLatestUnusedOtpAsync(int userId);

    Task InvalidatePreviousOtpsAsync(int userId);

    Task<Otp> CreateAsync(Otp otp);

    Task UpdateAsync(Otp otp);
}