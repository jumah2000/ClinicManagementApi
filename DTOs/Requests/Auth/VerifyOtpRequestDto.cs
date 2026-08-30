namespace ClinicManagementAPI.DTOs.Requests.Auth;

public class VerifyOtpRequestDto
{
        public string Email { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
}