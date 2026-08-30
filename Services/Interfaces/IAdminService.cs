using ClinicManagementAPI.DTOs.Requests.Admin;
using ClinicManagementAPI.DTOs.Responses.Admin;

namespace ClinicManagementAPI.Services.Interfaces;

public interface IAdminService
{
    Task<AdminResponseDto> CreateAsync(AdminCreateRequestDto request);

    Task<IEnumerable<AdminResponseDto>> GetAllAsync();

    Task<AdminResponseDto?> GetByUserIdAsync(string userId);

    Task<bool> UpdateAsync(int id, AdminUpdateRequestDto request);

    Task<bool> DeleteAsync(int id);
}