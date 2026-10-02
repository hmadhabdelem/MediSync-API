using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.DTOs.Prescriptions
{
    public class CreatePrescriptionItemDto
    {
        public String MedicationName { get; set; } = string.Empty;
        public String Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public String Duration { get; set; } = string.Empty;
    }
}
