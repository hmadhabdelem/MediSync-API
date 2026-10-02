using FluentValidation;
using MediSync.Application.DTOs.Prescriptions;
namespace MediSync.Application.DTOs.Validators;

public class CreatePrescriptionItemDtoValidator : AbstractValidator<CreatePrescriptionItemDto>
{
    public CreatePrescriptionItemDtoValidator()
    {
        RuleFor(x => x.MedicationName).NotEmpty().WithMessage("Medication name is required.").MaximumLength(150).WithMessage("Medication name must not exceed 150 characters.");
        RuleFor(x => x.Dosage)
            .NotEmpty().WithMessage("Dosage is required.");

        RuleFor(x => x.Frequency)
            .NotEmpty().WithMessage("Frequency is required.");

        RuleFor(x => x.Duration)
            .NotEmpty().WithMessage("Duration is required.");
    }
}