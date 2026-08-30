using ClinicManagementAPI.DTOs.Requests.Appointment;
using ClinicManagementAPI.DTOs.Responses.Appointment;

namespace ClinicManagementAPI.Services.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponseDto> CreateAsync(AppointmentCreateRequestDto request);

    Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();

    Task<AppointmentResponseDto?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, AppointmentUpdateRequestDto request);

    Task<bool> DeleteAsync(int id);
}