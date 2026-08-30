using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Repository.Interfaces;

public interface IDoctorRepository
{
        Task<Doctor?> GetByIdAsync(int id);

        Task<Doctor?> GetByUserIdAsync(int userId);

        Task<IEnumerable<Doctor>> GetAllAsync();

        Task<Doctor> CreateAsync(Doctor doctor);

        Task UpdateAsync(Doctor doctor);

        Task DeleteAsync(int id);
}