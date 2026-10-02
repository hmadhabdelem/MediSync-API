using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.DTOs.Reminders
{
    public class CreateReminderDto
    {
        public Guid PrescriptionITemID { get; set; } 
        public DateTime ScheduleTime { get; set; } 



    }
}
