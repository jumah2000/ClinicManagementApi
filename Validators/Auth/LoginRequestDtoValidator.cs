using ClinicManagementAPI.DTOs.Requests.Auth;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Auth;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(100);
        }
}