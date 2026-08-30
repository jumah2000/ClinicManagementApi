using ClinicManagementAPI.DTOs.Requests.Patient;
using ClinicManagementAPI.DTOs.Responses.Patient;

namespace ClinicManagementAPI.Services.Interfaces;

public interface IPatientService
{
    Task<PatientResponseDto> CreateAsync(PatientCreateRequestDto request);

    Task<IEnumerable<PatientResponseDto>> GetAllAsync();

    Task<PatientResponseDto?> GetByUserIdAsync(string userId);

    Task<bool> UpdateAsync(int id, PatientUpdateRequestDto request);

    Task<bool> DeleteAsync(int id);
}