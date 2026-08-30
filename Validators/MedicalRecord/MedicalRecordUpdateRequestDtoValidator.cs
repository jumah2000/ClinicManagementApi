using ClinicManagementAPI.DTOs.Requests.MedicalRecord;
using FluentValidation;

namespace ClinicManagementAPI.Validators.MedicalRecord;

public class MedicalRecordUpdateRequestDtoValidator : AbstractValidator<MedicalRecordUpdateRequestDto>
{
        public MedicalRecordUpdateRequestDtoValidator()
        {
            RuleFor(x => x.Diagnosis)
                .MaximumLength(500);

            RuleFor(x => x.Notes)
                .MaximumLength(2000);

            RuleFor(x => x.RecordDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.RecordDate.HasValue)
                .WithMessage("Record date cannot be in the future.");
        }
}