using FluentValidation;
using MediSync.Application.DTOs;
using MediSync.Application.DTOs.Appointments;

namespace MediSync.Application.DTOs.Validators
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(x => x.DoctorID)
                .NotEmpty().WithMessage("DoctorId is required.");
                
            RuleFor(x => x.AppointmentDate)
                .NotEmpty().WithMessage("AppointmentDate is required.")
                .Must(date => date > DateTime.UtcNow).WithMessage("AppointmentDate must be in the future.");
        }
    }
}
