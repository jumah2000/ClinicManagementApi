using ClinicManagementAPI.DTOs.Requests.MedicalRecord;
using FluentValidation;

namespace ClinicManagementAPI.Validators.MedicalRecord;

public class MedicalRecordCreateRequestDtoValidator : AbstractValidator<MedicalRecordCreateRequestDto>
{
        public MedicalRecordCreateRequestDtoValidator()
        {
            RuleFor(x => x.PatientUserId)
                .NotEmpty();

            RuleFor(x => x.DoctorUserId)
                .NotEmpty();

            RuleFor(x => x.Diagnosis)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Notes)
                .MaximumLength(2000);

            RuleFor(x => x.RecordDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Record date cannot be in the future.");
        }
}