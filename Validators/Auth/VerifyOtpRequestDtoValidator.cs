using ClinicManagementAPI.DTOs.Requests.Auth;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Auth;

public class VerifyOtpRequestDtoValidator : AbstractValidator<VerifyOtpRequestDto>
{
        public VerifyOtpRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.Otp)
                .NotEmpty()
                .Length(6)
                .Matches("^[0-9]+$")
                .WithMessage("OTP must be a 6-digit number.");
        }
}