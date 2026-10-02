using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using MediSync.Application.DTOs.Auth;
using MediSync.Application.Interfaces;
using MediSync.Domain.Entities;
using MediSync.Domain.Enums;
using MediSync.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace MediSync.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
       private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }
            return GenerateAuthResponse(user);

        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if(await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new Exception("Email already exists");
            }
            var user =new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                PhoneNumber = registerDto.PhoneNumber,
                Role = registerDto.Role,
               
            };
            _context.Users.Add(user);
            if(registerDto.Role == UserRole.Patient)
            {
                var patient = new Patient
                {
                    UserID = user.Id,
                    ChronicDiseases = registerDto.ChronicDisease,
                    BloodType = registerDto.BloodType,
                    EmergencyContact = registerDto.EmergencyContact
                };
                _context.Patients.Add(patient);
            }
            else if (registerDto.Role == UserRole.Doctor)
            {
                var doctor = new Doctor
                {
                    UserID = user.Id,
                    Specialization = registerDto.Specialization,
                    LicenseNumber = registerDto.LicenseNNumber,
                    Hospital = registerDto.Hospital
                };
                _context.Doctors.Add(doctor);
            }
            await _context.SaveChangesAsync();
            return GenerateAuthResponse(user);
    }

        private AuthResponseDto GenerateAuthResponse(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretkey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);


            var claims = new List<Claim>
            {

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            // Generate JWT token here
            var expiration = DateTime.UtcNow.AddHours(double.Parse(jwtSettings["DurationHours"] ?? "8"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretkey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                ExpirseAt = expiration
            };
        }
        }
    }
