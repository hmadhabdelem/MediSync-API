using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.DTOs.Appointments
{
    public class AvailablesDto
    {
        public DateTime SlotDateTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
