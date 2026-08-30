using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Repository.Interfaces;

public interface IAdminRepository
{
    Task<Admin?> GetByIdAsync(int id);

    Task<Admin?> GetByUserIdAsync(int userId);

    Task<IEnumerable<Admin>> GetAllAsync();

    Task<Admin> CreateAsync(Admin admin);

    Task UpdateAsync(Admin admin);

    Task DeleteAsync(int id);
}