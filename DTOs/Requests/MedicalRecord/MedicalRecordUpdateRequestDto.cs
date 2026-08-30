namespace ClinicManagementAPI.DTOs.Requests.MedicalRecord;

public class MedicalRecordUpdateRequestDto
{
    public string? Diagnosis { get; set; }

    public string? Notes { get; set; }

    public DateTime? RecordDate { get; set; }
}