using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Repository.Interfaces;

public interface IReceptionistRepository
{
    Task<Receptionist?> GetByIdAsync(int id);

    Task<Receptionist?> GetByUserIdAsync(int userId);

    Task<IEnumerable<Receptionist>> GetAllAsync();

    Task<Receptionist> CreateAsync(Receptionist receptionist);

    Task UpdateAsync(Receptionist receptionist);

    Task DeleteAsync(int id);
}