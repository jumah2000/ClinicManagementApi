namespace ClinicManagementAPI.DTOs.Responses.Appointment;

public class AppointmentResponseDto
{
    public int Id { get; set; }

    public string PatientUserId { get; set; } = string.Empty;

    public string DoctorUserId { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}