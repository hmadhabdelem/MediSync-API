using MediSync.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Domain.Entities
{
    public class Prescription
    {

        public Guid id { get; set; } = Guid.NewGuid();
        public Guid PatientID { get; set; }
        public Patient Patient { get; set; } = null;
        public Guid DoctorId { get; set; } 
        public Doctor Doctor { get; set; } = null;
        public string? Diagnosis { get; set; } 
        public PrescriptionStatus Status { get; set; } = PrescriptionStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
    }
}
