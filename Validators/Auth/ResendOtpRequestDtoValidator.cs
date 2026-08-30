using ClinicManagementAPI.DTOs.Requests.Auth;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Auth;

public class ResendOtpRequestDtoValidator : AbstractValidator<ResendOtpRequestDto>
{
        public ResendOtpRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);
        }
}