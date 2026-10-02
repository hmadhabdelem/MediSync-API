using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }
        public User user { get; set; } = null;
        public string ChronicDiseases { get; set; } = string.Empty;
        public string BloodType { get; set; } = string.Empty;
        public string EmergencyContact { get; set; } = string.Empty;
        public ICollection<Prescription> prescriptions { get; set; } = new List<Prescription>();
    }
}
