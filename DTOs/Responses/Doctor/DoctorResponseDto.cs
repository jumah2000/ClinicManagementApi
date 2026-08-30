namespace ClinicManagementAPI.DTOs.Responses.Doctor;

public class DoctorResponseDto
{
    public string UserId { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

}