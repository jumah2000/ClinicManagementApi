using ClinicManagementAPI.DTOs.Requests.Appointment;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Appointment;

public class AppointmentCreateRequestDtoValidator : AbstractValidator<AppointmentCreateRequestDto>
{
        public AppointmentCreateRequestDtoValidator()
        {
            RuleFor(x => x.PatientUserId)
                .NotEmpty();

            RuleFor(x => x.DoctorUserId)
                .NotEmpty();

            RuleFor(x => x.AppointmentDate)
                .NotEmpty()
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Appointment cannot be scheduled in the past.");
        }
}