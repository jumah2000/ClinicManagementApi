using ClinicManagementAPI.DTOs.Requests.MedicalRecord;
using ClinicManagementAPI.DTOs.Responses.MedicalRecord;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Repository.Interfaces;
using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IUserRepository userRepository, IPatientRepository patientRepository,
    IDoctorRepository doctorRepository, IAuditLogService auditLogService) : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _medicalRecordRepository = medicalRecordRepository;

    private readonly IUserRepository _userRepository = userRepository;

    private readonly IPatientRepository _patientRepository = patientRepository;

    private readonly IDoctorRepository _doctorRepository = doctorRepository;

    private readonly IAuditLogService _auditLogService = auditLogService;


    public async Task<MedicalRecordResponseDto> CreateAsync(
        MedicalRecordCreateRequestDto request)
    {
        // Find patient user
        var patientUser =
            await _userRepository.GetByUserIdAsync(request.PatientUserId);

        if (patientUser == null)
            throw new KeyNotFoundException("Patient user not found.");

        if (patientUser.UserType != "Patient")
            throw new InvalidOperationException(
                "The selected user is not a patient.");

        // Find patient profile
        var patient =
            await _patientRepository.GetByUserIdAsync(patientUser.Id);

        if (patient == null)
            throw new KeyNotFoundException(
                "Patient profile not found.");


        // Find doctor user
        var doctorUser =
            await _userRepository.GetByUserIdAsync(request.DoctorUserId);

        if (doctorUser == null)
            throw new KeyNotFoundException("Doctor user not found.");

        if (doctorUser.UserType != "Doctor")
            throw new InvalidOperationException(
                "The selected user is not a doctor.");

        // Find doctor profile
        var doctor =
            await _doctorRepository.GetByUserIdAsync(doctorUser.Id);

        if (doctor == null)
            throw new KeyNotFoundException(
                "Doctor profile not found.");


        // Record date cannot be in the future
        if (request.RecordDate > DateTime.UtcNow)
            throw new InvalidOperationException(
                "Medical record date cannot be in the future.");


        var medicalRecord = new MedicalRecord
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            Diagnosis = request.Diagnosis,
            Notes = request.Notes,
            RecordDate = request.RecordDate,
            CreatedAt = DateTime.UtcNow
        };

        var createdRecord =
            await _medicalRecordRepository.CreateAsync(medicalRecord);


        await _auditLogService.CreateAsync(
            "Create Medical Record",
            "Success",
            $"Medical record created for patient {patientUser.UserId} by doctor {doctorUser.UserId}.",
            doctorUser.Id);


        // Reload the record so Patient and Doctor navigation properties
        // are available for the response.
        var savedRecord =
            await _medicalRecordRepository.GetByIdAsync(createdRecord.Id);

        return MapToResponse(savedRecord ?? createdRecord);
    }


    public async Task<IEnumerable<MedicalRecordResponseDto>> GetAllAsync()
    {
        var records =
            await _medicalRecordRepository.GetAllAsync();

        return records.Select(MapToResponse);
    }


    public async Task<MedicalRecordResponseDto?> GetByIdAsync(int id)
    {
        var record =
            await _medicalRecordRepository.GetByIdAsync(id);

        if (record == null)
            return null;

        return MapToResponse(record);
    }


    public async Task<IEnumerable<MedicalRecordResponseDto>>
        GetByPatientUserIdAsync(string userId)
    {
        var user =
            await _userRepository.GetByUserIdAsync(userId);

        if (user == null)
            throw new KeyNotFoundException("Patient user not found.");

        if (user.UserType != "Patient")
            throw new InvalidOperationException(
                "The selected user is not a patient.");

        var records =
            await _medicalRecordRepository.GetByPatientUserIdAsync(userId);

        return records.Select(MapToResponse);
    }


    public async Task<IEnumerable<MedicalRecordResponseDto>>
        GetByDoctorUserIdAsync(string userId)
    {
        var user =
            await _userRepository.GetByUserIdAsync(userId);

        if (user == null)
            throw new KeyNotFoundException("Doctor user not found.");

        if (user.UserType != "Doctor")
            throw new InvalidOperationException(
                "The selected user is not a doctor.");

        var records =
            await _medicalRecordRepository.GetByDoctorUserIdAsync(userId);

        return records.Select(MapToResponse);
    }


    public async Task<bool> UpdateAsync(
        int id,
        MedicalRecordUpdateRequestDto request)
    {
        var record =
            await _medicalRecordRepository.GetByIdAsync(id);

        if (record == null)
            return false;


        if (request.RecordDate.HasValue)
        {
            if (request.RecordDate.Value > DateTime.UtcNow)
                throw new InvalidOperationException(
                    "Medical record date cannot be in the future.");

            record.RecordDate = request.RecordDate.Value;
        }


        if (request.Diagnosis != null)
            record.Diagnosis = request.Diagnosis;

        if (request.Notes != null)
            record.Notes = request.Notes;

        record.UpdatedAt = DateTime.UtcNow;


        await _medicalRecordRepository.UpdateAsync(record);


        await _auditLogService.CreateAsync(
            "Update Medical Record",
            "Success",
            $"Medical record {record.Id} was updated.",
            record.Doctor?.User?.Id);


        return true;
    }


    private static MedicalRecordResponseDto MapToResponse(
        MedicalRecord record)
    {
        return new MedicalRecordResponseDto
        {
            Id = record.Id,

            PatientUserId =
                record.Patient?.User?.UserId ?? string.Empty,

            DoctorUserId =
                record.Doctor?.User?.UserId ?? string.Empty,

            Diagnosis = record.Diagnosis,

            Notes = record.Notes,

            RecordDate = record.RecordDate,

            CreatedAt = record.CreatedAt,

            UpdatedAt = record.UpdatedAt
        };
    }
}