using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.DTOs.Prescriptions
{
    public class PrescriptionDto
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public string PatientNAme { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string? Diagnosis { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PrescriptionItemDto> Items { get; set; } = new();

    }
}
