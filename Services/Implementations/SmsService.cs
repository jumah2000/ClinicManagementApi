using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class SmsService(ILogger<SmsService> logger) : ISmsService
{
        private readonly ILogger<SmsService> _logger = logger;

        public Task SendOtpAsync(string phoneNumber, string otp)
        {
            _logger.LogInformation(
                "OTP for {PhoneNumber}: {Otp}", phoneNumber, otp);

            return Task.CompletedTask;
        }
}