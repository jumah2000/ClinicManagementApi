using ClinicManagementAPI.DTOs.Requests.Receptionist;
using FluentValidation;

namespace ClinicManagementAPI.Validators.Receptionist;

public class ReceptionistCreateRequestDtoValidator : AbstractValidator<ReceptionistCreateRequestDto>
{
        public ReceptionistCreateRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
}