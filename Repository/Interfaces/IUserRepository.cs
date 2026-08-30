using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Repository.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByUserIdAsync(string userId);

    Task<User?> GetByEmailAsync(string email);

    Task<IEnumerable<User>> GetByUserTypeAsync(string userType);

    Task<User?> GetLatestUserByTypeAsync(string userType);

    Task<User> CreateAsync(User user);

    Task UpdateAsync(User user);
}