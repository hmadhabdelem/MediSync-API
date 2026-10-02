using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Domain.Entities
{
    public class PrescriptionItem
    {

        public Guid id { get; set; } = Guid.NewGuid();
        public Guid PrescirptionId { get; set; }
        public Prescription Prescription { get; set; } = null;
        public string MedicineName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string FrequencyPerDay { get; set; }= string.Empty;
        public string DurationDays { get; set; }=string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    }
}
