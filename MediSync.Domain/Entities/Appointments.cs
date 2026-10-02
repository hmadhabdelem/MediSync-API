using MediSync.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Domain.Entities
{
    public class Appointments
    {
        [Key]
        public Guid ID { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null;
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null;
        public DateTime AppointmentDate { get; set; }
        public  AppointmentStatus  status{ get; set; } = AppointmentStatus.Pending;
        public string? Notes { get; set; }
    }
}
