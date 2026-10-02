using FluentValidation;
using MediSync.Application.DTOs.Appointments;
namespace MediSync.Application.DTOs.Validators;

public class CreateScheduleDtoValidator : AbstractValidator<CreateScheduleDto>
{
    public CreateScheduleDtoValidator()
    {
        RuleFor(x => x.DayOfWeek).IsInEnum().WithMessage("A valid DayOfWeek is required.");
        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("StartTime is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("EndTime is required.")
            .GreaterThan(x => x.StartTime).WithMessage("EndTime must be after StartTime.");

        RuleFor(x => x.SlotDurationMinutes)
            .GreaterThan(0).WithMessage("SlotDurationMinutes must be greater than 0.");
    }
}