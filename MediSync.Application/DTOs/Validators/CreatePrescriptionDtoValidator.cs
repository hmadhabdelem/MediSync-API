using FluentValidation;
using MediSync.Application.DTOs.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.DTOs.Validators
{
    public class CreatePrescriptionDtoValidator : AbstractValidator<CreatePrescriptionDto>
    {
        public CreatePrescriptionDtoValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("AppointmentId is required.");

            RuleFor(x => x.Item)
                .NotEmpty().WithMessage("Item is required.");

            RuleForEach(x => x.Item).ChildRules(item =>
            {
                item.RuleFor(i => i.MedicationName)
                .NotEmpty().WithMessage("MedicationName is required.");
                item.RuleFor(i => i.Dosage)
                    .NotEmpty().WithMessage("Dosage is required.");
                item.RuleFor(i => i.Frequency)
                    .NotEmpty().WithMessage("Frequency is required.");
            });
        }
    }
}
