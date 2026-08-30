namespace ClinicManagementAPI.DTOs.Responses.Auth;

public class LoginResponseDto
{
    public string UserId { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string UserType { get; set; } = string.Empty;

    public DateTime? LastLoginAt { get; set; }
}