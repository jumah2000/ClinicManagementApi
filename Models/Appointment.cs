using ClinicManagementAPI.Enum;

namespace ClinicManagementAPI.Models;

public class Appointment
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public DateTime AppointmentDate { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}