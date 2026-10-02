using MediSync.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; }= string.Empty;
        public string PasswordHash { get; set; }=string.Empty;
        public string PhoneNumber { get; set; }=string.Empty ;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;


        public Patient? patient { get; set; }
        public Doctor? doctor { get; set; }
    }
}
