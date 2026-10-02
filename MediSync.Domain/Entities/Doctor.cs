using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Domain.Entities
{
    public class Doctor
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }
        public User user { get; set; } = null;
        public string Specialization { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string Hospital { get; set; } = string.Empty;
        public ICollection<Prescription> prescriptions { get; set; } = new List<Prescription>();
    }
}
