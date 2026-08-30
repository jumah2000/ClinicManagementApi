using ClinicManagementAPI.DTOs.Requests.Doctor;
using ClinicManagementAPI.DTOs.Responses.Doctor;

namespace ClinicManagementAPI.Services.Interfaces;

public interface IDoctorService
{
    Task<DoctorResponseDto> CreateAsync(DoctorCreateRequestDto request);

    Task<IEnumerable<DoctorResponseDto>> GetAllAsync();

    Task<DoctorResponseDto?> GetByUserIdAsync(string userId);

    Task<bool> UpdateAsync(int id, DoctorUpdateRequestDto request);

    Task<bool> DeleteAsync(int id);
}