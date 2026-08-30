namespace ClinicManagementAPI.DTOs.Requests.Appointment;

public class AppointmentCreateRequestDto
{
    public string PatientUserId { get; set; } = string.Empty;

    public string DoctorUserId { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }
}