using ClinicManagementAPI.DTOs.Requests.Doctor;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Doctor;

public class DoctorUpdateRequestDtoValidator : AbstractValidator<DoctorUpdateRequestDto>
{
        public DoctorUpdateRequestDtoValidator()
        {
            RuleFor(x => x.Specialization)
                .MaximumLength(150);

            RuleFor(x => x.LicenseNumber)
                .MaximumLength(100);
        }
}