using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.DTOs.Reminders
{
    public class ReminderDto
    {
        public Guid ID { get; set; }
        public Guid PrescriptionITemID { get; set; }
        public string Status { get; set; } = string.Empty;
        public string MedicationName { get; set; } = string.Empty;
        public DateTime ScheduleTime { get; set; }

    }
}
