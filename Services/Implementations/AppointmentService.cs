using ClinicManagementAPI.DTOs.Requests.Appointment;
using ClinicManagementAPI.DTOs.Responses.Appointment;
using ClinicManagementAPI.Enum;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class AppointmentService(IAppointmentRepository appointmentRepository, IUserRepository userRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository, IAuditLogService auditLogService) : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPatientRepository _patientRepository = patientRepository;
    private readonly IDoctorRepository _doctorRepository = doctorRepository;
    private readonly IAuditLogService _auditLogService = auditLogService;

    public async Task<AppointmentResponseDto> CreateAsync(
        AppointmentCreateRequestDto request)
    {
        var patientUser =
            await _userRepository.GetByUserIdAsync(request.PatientUserId);

        if (patientUser == null)
            throw new KeyNotFoundException("Patient user not found.");

        if (patientUser.UserType != "Patient")
            throw new InvalidOperationException(
                "The selected user is not a patient.");

        var patient =
            await _patientRepository.GetByUserIdAsync(patientUser.Id);

        if (patient == null)
            throw new KeyNotFoundException("Patient profile not found.");

        var doctorUser =
            await _userRepository.GetByUserIdAsync(request.DoctorUserId);

        if (doctorUser == null)
            throw new KeyNotFoundException("Doctor user not found.");

        if (doctorUser.UserType != "Doctor")
            throw new InvalidOperationException(
                "The selected user is not a doctor.");

        var doctor =
            await _doctorRepository.GetByUserIdAsync(doctorUser.Id);

        if (doctor == null)
            throw new KeyNotFoundException("Doctor profile not found.");

        if (request.AppointmentDate <= DateTime.UtcNow)
            throw new InvalidOperationException(
                "Appointment cannot be scheduled in the past.");

        var doctorConflict =
            await _appointmentRepository.HasDoctorConflictAsync(
                doctor.Id,
                request.AppointmentDate);

        if (doctorConflict)
            throw new InvalidOperationException(
                "Doctor already has an appointment at this time.");

        var patientConflict =
            await _appointmentRepository.HasPatientConflictAsync(
                patient.Id,
                doctor.Id,
                request.AppointmentDate);

        if (patientConflict)
            throw new InvalidOperationException(
                "Patient already has an appointment with this doctor at this time.");

        var appointment = new Appointment
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            AppointmentDate = request.AppointmentDate,
            Status = AppointmentStatus.Scheduled,
            CreatedAt = DateTime.UtcNow
        };

        var createdAppointment =
            await _appointmentRepository.CreateAsync(appointment);

        await _auditLogService.CreateAsync(
            "Create Appointment",
            "Success",
            $"Appointment created for patient {patientUser.UserId} with doctor {doctorUser.UserId}.",
            patientUser.Id);

        return MapToResponse(createdAppointment);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
    {
        var appointments =
            await _appointmentRepository.GetAllAsync();

        return appointments.Select(MapToResponse);
    }

    public async Task<AppointmentResponseDto?> GetByIdAsync(int id)
    {
        var appointment =
            await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
            return null;

        return MapToResponse(appointment);
    }

  public async Task<bool> UpdateAsync(
    int id,
    AppointmentUpdateRequestDto request)
{
    var appointment = await _appointmentRepository.GetByIdAsync(id);

    if (appointment == null)
        return false;

    if (request.AppointmentDate.HasValue)
    {
        if (request.AppointmentDate.Value <= DateTime.UtcNow)
            throw new InvalidOperationException(
                "Appointment cannot be scheduled in the past.");

        var doctorConflict =
            await _appointmentRepository.HasDoctorConflictAsync(
                appointment.DoctorId,
                request.AppointmentDate.Value,
                appointment.Id);

        if (doctorConflict)
            throw new InvalidOperationException(
                "Doctor already has an appointment at this time.");

        var patientConflict =
            await _appointmentRepository.HasPatientConflictAsync(
                appointment.PatientId,
                appointment.DoctorId,
                request.AppointmentDate.Value,
                appointment.Id);

        if (patientConflict)
            throw new InvalidOperationException(
                "Patient already has an appointment with this doctor at this time.");

        appointment.AppointmentDate =
            request.AppointmentDate.Value;
    }

    if (!string.IsNullOrWhiteSpace(request.Status))
    {
        if (!System.Enum.TryParse<AppointmentStatus>(
                request.Status,
                true,
                out var newStatus))
        {
            throw new InvalidOperationException(
                "Invalid appointment status.");
        }

        if (appointment.Status == AppointmentStatus.Cancelled &&
            newStatus == AppointmentStatus.Completed)
        {
            throw new InvalidOperationException(
                "A cancelled appointment cannot be completed.");
        }

        appointment.Status = newStatus;
    }

    appointment.UpdatedAt = DateTime.UtcNow;

    await _appointmentRepository.UpdateAsync(appointment);

    await _auditLogService.CreateAsync(
        "Update Appointment",
        "Success",
        $"Appointment {appointment.Id} was updated.",
        appointment.Patient?.User?.Id);

    return true;
}

    public async Task<bool> DeleteAsync(int id)
    {
        var appointment =
            await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null)
            return false;

        if (appointment.Status == AppointmentStatus.Cancelled)
            return false;

        await _appointmentRepository.DeleteAsync(id);

        await _auditLogService.CreateAsync(
            "Cancel Appointment",
            "Success",
            $"Appointment {appointment.Id} was cancelled.",
            appointment.Patient?.User?.Id);

        return true;
    }

    private static AppointmentResponseDto MapToResponse(
        Appointment appointment)
    {
        return new AppointmentResponseDto
        {
            Id = appointment.Id,

            PatientUserId =
                appointment.Patient?.User?.UserId ?? string.Empty,

            DoctorUserId =
                appointment.Doctor?.User?.UserId ?? string.Empty,

            AppointmentDate = appointment.AppointmentDate,

            Status = appointment.Status.ToString(),

            CreatedAt = appointment.CreatedAt,

            UpdatedAt = appointment.UpdatedAt
        };
    }
}