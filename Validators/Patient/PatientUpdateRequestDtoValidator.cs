using ClinicManagementAPI.DTOs.Requests.Patient;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Patient;

public class PatientUpdateRequestDtoValidator : AbstractValidator<PatientUpdateRequestDto>
{
        public PatientUpdateRequestDtoValidator()
        {
            RuleFor(x => x.Gender)
                .MaximumLength(20);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .MaximumLength(500);
        }
}