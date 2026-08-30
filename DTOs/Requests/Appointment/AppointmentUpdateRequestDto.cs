namespace ClinicManagementAPI.DTOs.Requests.Appointment;

public class AppointmentUpdateRequestDto
{
    public DateTime? AppointmentDate { get; set; }

    public string? Status { get; set; }
}