namespace ClinicManagementAPI.Models;

public class Doctor
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string Specialization { get; set; }= string.Empty;

    public string LicenseNumber { get; set; }= string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}