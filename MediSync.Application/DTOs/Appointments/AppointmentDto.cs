using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.DTOs.Appointments
{
    public class AppointmentDto
    {
        public Guid ID { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }= string.Empty;
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }=string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }

    }
}
