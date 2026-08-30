using ClinicManagementAPI.DTOs.Requests.Appointment;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Appointment;

public class AppointmentUpdateRequestDtoValidator : AbstractValidator<AppointmentUpdateRequestDto>
{
        public AppointmentUpdateRequestDtoValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.AppointmentDate.HasValue)
                .WithMessage("Appointment cannot be scheduled in the past.");

            RuleFor(x => x.Status)
                .Must(status => 
                    status.Equals("Scheduled", StringComparison.OrdinalIgnoreCase) ||
                    status.Equals("Completed", StringComparison.OrdinalIgnoreCase) ||
                    status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                .When(x => !string.IsNullOrEmpty(x.Status))
                .WithMessage("Status must be Scheduled, Completed, or Cancelled.");
        }  
}