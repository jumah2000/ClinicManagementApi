namespace ClinicManagementAPI.DTOs.Requests.Patient;

public class PatientUpdateRequestDto
{
        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
}