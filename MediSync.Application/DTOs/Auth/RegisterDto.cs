using MediSync.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.DTOs.Auth
{
    public class RegisterDto
    {
        public string FullName { get; set; }=string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }=string.Empty;
        public string PhoneNumber { get; set; }= string.Empty;
        public UserRole Role { get; set; }


        public string? ChronicDisease { get; set; }
        public string? BloodType { get; set; }
        public string? EmergencyContact { get; set; }


        public string? Specialization { get; set; }
        public string? LicenseNNumber { get; set; }
        public string? Hospital { get; set; }






    }
}
