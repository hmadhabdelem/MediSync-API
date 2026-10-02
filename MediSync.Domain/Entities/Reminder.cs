using MediSync.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Domain.Entities
{
    public class Reminder
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid Userid { get; set; }
        public Guid PrescriptionItemId { get; set; }
        public PrescriptionItem? PrescriptionItem { get; set; }
        public DateTime ScheduledTime { get; set; }
        public ReminderStatus status { get; set; }=ReminderStatus.Pending;
        public DateTime? TakenAt { get; set; } 


    }
}
