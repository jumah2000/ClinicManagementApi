using ClinicManagementAPI.DTOs.Requests.Receptionist;
using ClinicManagementAPI.DTOs.Responses.Receptionist;

namespace ClinicManagementAPI.Services.Interfaces;

public interface IReceptionistService
{
    Task<ReceptionistResponseDto> CreateAsync(ReceptionistCreateRequestDto request);

    Task<IEnumerable<ReceptionistResponseDto>> GetAllAsync();

    Task<ReceptionistResponseDto?> GetByUserIdAsync(string userId);

    Task<bool> UpdateAsync(int id, ReceptionistUpdateRequestDto request);

    Task<bool> DeleteAsync(int id);
}