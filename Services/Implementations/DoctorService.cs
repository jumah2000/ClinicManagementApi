using ClinicManagementAPI.DTOs.Requests.Doctor;
using ClinicManagementAPI.DTOs.Responses.Doctor;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class DoctorService(IDoctorRepository doctorRepository, IUserRepository userRepository, IAuditLogService auditLogService) : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository = doctorRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuditLogService _auditLogService = auditLogService;

    public async Task<DoctorResponseDto> CreateAsync(DoctorCreateRequestDto request)
    {
        var user = await _userRepository.GetByUserIdAsync(request.UserId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (user.UserType != "Doctor")
            throw new InvalidOperationException("The user is not a doctor.");

        var existingDoctor = await _doctorRepository.GetByUserIdAsync(user.Id);

        if (existingDoctor != null)
            throw new InvalidOperationException("Doctor already exists.");

        var doctor = new Doctor
        {
            UserId = user.Id,
            Specialization = request.Specialization,
            LicenseNumber = request.LicenseNumber,
            CreatedAt = DateTime.UtcNow
        };

        var createdDoctor = await _doctorRepository.CreateAsync(doctor);

        await _auditLogService.CreateAsync(
            "Create Doctor",
            "Success",
            $"Doctor {user.UserId} was created.",
            user.Id);

        return MapToResponse(createdDoctor);
    }

    public async Task<IEnumerable<DoctorResponseDto>> GetAllAsync()
    {
        var doctors = await _doctorRepository.GetAllAsync();
        return doctors.Select(MapToResponse);
    }

    public async Task<DoctorResponseDto?> GetByUserIdAsync(string userId)
    {
        var user = await _userRepository.GetByUserIdAsync(userId);

        if (user == null)
            return null;

        var doctor = await _doctorRepository.GetByUserIdAsync(user.Id);

        if (doctor == null)
            return null;

        return MapToResponse(doctor);
    }

    public async Task<bool> UpdateAsync(int id, DoctorUpdateRequestDto request)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor == null)
            return false;

        // Load user for audit
        var user = await _userRepository.GetByIdAsync(doctor.UserId);
        var userBusinessId = user?.UserId ?? "Unknown";

        doctor.Specialization = request.Specialization ?? doctor.Specialization;
        doctor.LicenseNumber = request.LicenseNumber ?? doctor.LicenseNumber;
        doctor.UpdatedAt = DateTime.UtcNow;

        await _doctorRepository.UpdateAsync(doctor);

        await _auditLogService.CreateAsync(
            "Update Doctor",
            "Success",
            $"Doctor {userBusinessId} was updated.",
            doctor.UserId);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor == null)
            return false;

        // Load user for audit
        var user = await _userRepository.GetByIdAsync(doctor.UserId);
        var userBusinessId = user?.UserId ?? "Unknown";

        await _doctorRepository.DeleteAsync(id);

        await _auditLogService.CreateAsync(
            "Deactivate Doctor",
            "Success",
            $"Doctor {userBusinessId} was deactivated.",
            doctor.UserId);

        return true;
    }

    private static DoctorResponseDto MapToResponse(Doctor doctor)
    {
        return new DoctorResponseDto
        {
            UserId = doctor.User?.UserId ?? string.Empty,
            FirstName = doctor.User?.FirstName ?? string.Empty,
            LastName = doctor.User?.LastName ?? string.Empty,
            Email = doctor.User?.Email ?? string.Empty,
            Specialization = doctor.Specialization,
            LicenseNumber = doctor.LicenseNumber,
            CreatedAt = doctor.CreatedAt,
            UpdatedAt = doctor.UpdatedAt
        };
    }
}