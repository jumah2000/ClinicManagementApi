using ClinicManagementAPI.DTOs.Requests.MedicalRecord;
using ClinicManagementAPI.DTOs.Responses.MedicalRecord;

namespace ClinicManagementAPI.Services.Interfaces;

public interface IMedicalRecordService
{
    Task<MedicalRecordResponseDto> CreateAsync(
        MedicalRecordCreateRequestDto request);

    Task<IEnumerable<MedicalRecordResponseDto>> GetAllAsync();

    Task<MedicalRecordResponseDto?> GetByIdAsync(int id);

    Task<IEnumerable<MedicalRecordResponseDto>> GetByPatientUserIdAsync(
        string userId);

    Task<IEnumerable<MedicalRecordResponseDto>> GetByDoctorUserIdAsync(
        string userId);

    Task<bool> UpdateAsync(int id, MedicalRecordUpdateRequestDto request);
}