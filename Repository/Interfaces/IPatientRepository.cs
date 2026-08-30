using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Repository.Interfaces;

public interface IPatientRepository
{
        Task<Patient?> GetByIdAsync(int id);

        Task<Patient?> GetByUserIdAsync(int userId);

        Task<IEnumerable<Patient>> GetAllAsync();

        Task<Patient> CreateAsync(Patient patient);

        Task UpdateAsync(Patient patient);

        Task DeleteAsync(int id);
}