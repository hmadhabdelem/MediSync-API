using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.DTOs.Prescriptions
{
    public class CreatePrescriptionDto
    {
        public Guid AppointmentId { get; set; }
        public string? Diagnosis { get; set; }
        public List<CreatePrescriptionItemDto> Item { get; set; } = new();
    }
}
