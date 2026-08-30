using ClinicManagementAPI.DTOs.Requests.Doctor;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Doctor;

public class DoctorCreateRequestDtoValidator : AbstractValidator<DoctorCreateRequestDto>
{
        public DoctorCreateRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Specialization)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.LicenseNumber)
                .NotEmpty()
                .MaximumLength(100);
        }
}