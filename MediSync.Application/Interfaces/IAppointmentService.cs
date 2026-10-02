using MediSync.Application.DTOs.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> BookAppointmentAsync(Guid PatientUserId, CreateAppointmentDto dto);
        Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsAsync(Guid PatientUerId);
        Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(Guid DoctorUerId);
        Task<bool> CancelAppointAsync(Guid AppointmentId, Guid UserId);


        Task<bool> AddDoctorScheduleAsync(Guid DoctorUserId, CreateScheduleDto dto);
        Task<IEnumerable<CreateScheduleDto>> GetDoctorSchedulesAsync(Guid doctorId);
        Task<IEnumerable<AvailablesDto>> GetAvailableSlotsAsync(Guid doctorId,DateTime date);


    }
}
