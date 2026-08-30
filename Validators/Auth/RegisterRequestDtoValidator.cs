using ClinicManagementAPI.DTOs.Requests.Auth;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Auth;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
        public RegisterRequestDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100);

            RuleFor(x => x.UserType)
                .NotEmpty()
                .MaximumLength(20)
                .Must(type =>
                    type.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                    type.Equals("Doctor", StringComparison.OrdinalIgnoreCase) ||
                    type.Equals("Receptionist", StringComparison.OrdinalIgnoreCase) ||
                    type.Equals("Patient", StringComparison.OrdinalIgnoreCase))
                .WithMessage("UserType must be Admin, Doctor, Receptionist, or Patient.");
        }
}