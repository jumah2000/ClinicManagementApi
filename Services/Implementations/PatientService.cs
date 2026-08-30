using ClinicManagementAPI.DTOs.Requests.Patient;
using ClinicManagementAPI.DTOs.Responses.Patient;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class PatientService(IPatientRepository patientRepository, IUserRepository userRepository, IAuditLogService auditLogService) : IPatientService
{
    private readonly IPatientRepository _patientRepository = patientRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuditLogService _auditLogService = auditLogService;

    public async Task<PatientResponseDto> CreateAsync(
        PatientCreateRequestDto request)
    {
        var user = await _userRepository.GetByUserIdAsync(request.UserId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (user.UserType != "Patient")
            throw new InvalidOperationException(
                "The user is not a patient.");

        var existingPatient =
            await _patientRepository.GetByUserIdAsync(user.Id);

        if (existingPatient != null)
            throw new InvalidOperationException(
                "Patient already exists.");

        var patient = new Patient
        {
            UserId = user.Id,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow
        };

        var createdPatient =
            await _patientRepository.CreateAsync(patient);

        await _auditLogService.CreateAsync(
            "Create Patient",
            "Success",
            $"Patient {user.UserId} was created.",
            user.Id);

        return MapToResponse(createdPatient);
    }

    public async Task<IEnumerable<PatientResponseDto>> GetAllAsync()
    {
        var patients = await _patientRepository.GetAllAsync();

        return patients.Select(MapToResponse);
    }

    public async Task<PatientResponseDto?> GetByUserIdAsync(
        string userId)
    {
        var user = await _userRepository.GetByUserIdAsync(userId);

        if (user == null)
            return null;

        var patient =
            await _patientRepository.GetByUserIdAsync(user.Id);

        if (patient == null)
            return null;

        return MapToResponse(patient);
    }

    public async Task<bool> UpdateAsync(
        int id,
        PatientUpdateRequestDto request)
    {
        var patient =
            await _patientRepository.GetByIdAsync(id);

        if (patient == null)
            return false;

        var user =
            await _userRepository.GetByIdAsync(patient.UserId);

        var userBusinessId =
            user?.UserId ?? "Unknown";

        patient.DateOfBirth =
            request.DateOfBirth ?? patient.DateOfBirth;

        patient.Gender =
            request.Gender ?? patient.Gender;

        patient.PhoneNumber =
            request.PhoneNumber ?? patient.PhoneNumber;

        patient.Address =
            request.Address ?? patient.Address;

        patient.UpdatedAt = DateTime.UtcNow;

        await _patientRepository.UpdateAsync(patient);

        await _auditLogService.CreateAsync(
            "Update Patient",
            "Success",
            $"Patient {userBusinessId} was updated.",
            patient.UserId);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient =
            await _patientRepository.GetByIdAsync(id);

        if (patient == null)
            return false;

        var user =
            await _userRepository.GetByIdAsync(patient.UserId);

        var userBusinessId =
            user?.UserId ?? "Unknown";

        await _patientRepository.DeleteAsync(id);

        await _auditLogService.CreateAsync(
            "Deactivate Patient",
            "Success",
            $"Patient {userBusinessId} was deactivated.",
            patient.UserId);

        return true;
    }

    private static PatientResponseDto MapToResponse(
        Patient patient)
    {
        return new PatientResponseDto
        {
            UserId = patient.User?.UserId ?? string.Empty,
            FirstName = patient.User?.FirstName ?? string.Empty,
            LastName = patient.User?.LastName ?? string.Empty,
            Email = patient.User?.Email ?? string.Empty,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            PhoneNumber = patient.PhoneNumber,
            Address = patient.Address,
            CreatedAt = patient.CreatedAt,
            UpdatedAt = patient.UpdatedAt
        };
    }
}
