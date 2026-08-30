using ClinicManagementAPI.DTOs.Requests.Patient;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Patient;

public class PatientCreateRequestDtoValidator : AbstractValidator<PatientCreateRequestDto>
{
        public PatientCreateRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Gender)
                .MaximumLength(20);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .MaximumLength(500);
        }
}