namespace ClinicManagementAPI.Models;

public class MedicalRecord
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public string Diagnosis { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public DateTime RecordDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; } 
}