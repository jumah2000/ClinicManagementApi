using ClinicManagementAPI.Services.Interfaces;

namespace ClinicManagementAPI.Services.Implementations;

public class EmailService(ILogger<EmailService> logger) : IEmailService
{
        private readonly ILogger<EmailService> _logger = logger;

        public Task SendOtpAsync(string email, string otp)
        {
            _logger.LogInformation("OTP for {Email}: {Otp}", email, otp);

            return Task.CompletedTask;
        }
}