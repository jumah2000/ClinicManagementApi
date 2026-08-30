namespace ClinicManagementAPI.DTOs.Requests.Patient;

public class PatientCreateRequestDto
{
        public string UserId { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
}