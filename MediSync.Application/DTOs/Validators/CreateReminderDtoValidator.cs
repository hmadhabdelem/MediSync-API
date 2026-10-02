using FluentValidation;
using MediSync.Application.DTOs.Reminders;
namespace MediSync.Application.DTOs.Validators;

public class CreateReminderDtoValidator : AbstractValidator<CreateReminderDto>
{
    public CreateReminderDtoValidator()
    {
        RuleFor(x => x.PrescriptionITemID).NotEmpty().WithMessage("PrescriptionItemId is required.");
        RuleFor(x => x.ScheduleTime)
            .NotEmpty().WithMessage("ScheduledTime is required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("ScheduledTime must be in the future.");
    }
}