namespace ClinicManagementAPI.Models;

public class Patient
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; } 

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}