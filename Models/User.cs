namespace ClinicManagementAPI.Models;

public class User
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string UserType { get; set; } = string.Empty;

    public bool IsVerified { get; set; }

    public bool IsActive { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Patient? Patient { get; set; }

    public Doctor? Doctor { get; set; }

    public Receptionist? Receptionist { get; set; }
    
    public string PhoneNumber { get; set; } = string.Empty;

    public Admin? Admin { get; set; }

    public ICollection<Otp> Otps { get; set; } = new List<Otp>();

    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}