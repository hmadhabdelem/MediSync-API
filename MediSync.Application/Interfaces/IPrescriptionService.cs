using MediSync.Application.DTOs.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.Interfaces
{
    public interface IPrescriptionService
    {
        Task<PrescriptionDto> CreatePrescriptionAsync(Guid doctorUserId, CreatePrescriptionDto dto);
        Task<IEnumerable<PrescriptionDto>> GetPatientPrescriptionsAsync(Guid patientUserId);
        Task<PrescriptionDto?> GetPrescriptionByAppointmentIdAsync(Guid appointmentId);
    }
}
