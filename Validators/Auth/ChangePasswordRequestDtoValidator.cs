using ClinicManagementAPI.DTOs.Requests.Auth;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Auth;

public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
{
        public ChangePasswordRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100)
                .Matches(@"[A-Z]")
                .WithMessage("New password must contain at least one uppercase letter.")
                .Matches(@"[a-z]")
                .WithMessage("New password must contain at least one lowercase letter.")
                .Matches(@"[0-9]")
                .WithMessage("New password must contain at least one number.")
                .Matches(@"[^a-zA-Z0-9]")
                .WithMessage("New password must contain at least one special character.");
        }
}