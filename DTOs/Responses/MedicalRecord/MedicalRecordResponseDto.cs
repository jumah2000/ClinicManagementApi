namespace ClinicManagementAPI.DTOs.Responses.MedicalRecord;

public class MedicalRecordResponseDto
{
    public int Id { get; set; }

    public string PatientUserId { get; set; } = string.Empty;

    public string DoctorUserId { get; set; } = string.Empty;

    public string Diagnosis { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public DateTime RecordDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}