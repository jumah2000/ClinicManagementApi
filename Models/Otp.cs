namespace ClinicManagementAPI.Models;

public class Otp
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    public string Code { get; set; } = string.Empty;
    
    public bool IsUsed { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
    public DateTime CreatedAt { get; set; }  
    
    public DateTime? VerifiedAt { get; set; }
}