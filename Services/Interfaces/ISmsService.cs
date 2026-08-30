namespace ClinicManagementAPI.Services.Interfaces;

public interface ISmsService
{
    Task SendOtpAsync(string phoneNumber, string otp);
}