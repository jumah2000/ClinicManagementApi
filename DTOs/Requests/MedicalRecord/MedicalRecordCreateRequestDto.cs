namespace ClinicManagementAPI.DTOs.Requests.MedicalRecord;

public class MedicalRecordCreateRequestDto
{
    public string PatientUserId { get; set; } = string.Empty;

    public string DoctorUserId { get; set; } = string.Empty;

    public string Diagnosis { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public DateTime RecordDate { get; set; }
}