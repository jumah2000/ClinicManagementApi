using ClinicManagementAPI.DTOs.Requests.Admin;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Admin;

public class AdminCreateRequestDtoValidator : AbstractValidator<AdminCreateRequestDto>
{
        public AdminCreateRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
}