using MediSync.Application.DTOs.Appointments;
using MediSync.Application.Interfaces;
using MediSync.Domain.Entities;
using MediSync.Domain.Enums;
using MediSync.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext context;
        public AppointmentService(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<AppointmentDto> BookAppointmentAsync(Guid PatientUserId, CreateAppointmentDto dto)
        {
            var patient = await context.Patients.FirstOrDefaultAsync(p => p.UserID == PatientUserId);
            if (patient == null)
            {
                throw new Exception("patient not found");
            }
            var doctor = await context.Doctors.Include(d => d.user).FirstOrDefaultAsync(d => d.Id == dto.DoctorID);
            if (doctor == null)
            {
                throw new Exception("doctor not found");
            }
            var isBooked = await context.Appointments.AnyAsync(a =>
            a.DoctorId == dto.DoctorID &&
            a.AppointmentDate == dto.AppointmentDate &&
            a.status != AppointmentStatus.Cancelled);

            if (isBooked)
            {
                throw new Exception("this time not avolopal for doctor ");
            }
            var appointment = new Appointments
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                AppointmentDate = dto.AppointmentDate,
                status = AppointmentStatus.Pending,
                Notes = dto.Notes
            };
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();
            return new AppointmentDto
            {
                ID = appointment.ID,
                PatientId = patient.Id,
                PatientName = patient.user?.FullName ?? "",
                DoctorId = doctor.Id,
                DoctorName = doctor.user.FullName,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.status.ToString(),
                Notes = appointment.Notes
            };

        }
        public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsAsync(Guid PatientUerId)
        {
            var patient = await context.Patients.FirstOrDefaultAsync(p=> p.UserID== PatientUerId);
            if (patient == null)
            {
                throw new Exception("patient not founding");
            }

            return await context.Appointments
                .Include(a=> a.Patient)
                .ThenInclude(p=> p.user)
                .Include(a=> a.Doctor)
                .ThenInclude(d=>d.user)
                .Where(a=>a.PatientId==patient.Id)
                .OrderByDescending(a=>a.AppointmentDate)
                .Select(a=>new AppointmentDto
            {
                ID=a.ID,
                PatientId= a.PatientId,
                PatientName = a.Patient.user.FullName,
                DoctorId= a.Doctor.Id,
                DoctorName= a.Doctor.user.FullName,
                AppointmentDate = a.AppointmentDate,
                Status= a.status.ToString(),
                Notes =a.Notes
            }).ToListAsync();

        }

        public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(Guid DoctorUerId)
        {
            var doctor = await context.Doctors.FirstOrDefaultAsync(d=> d.UserID== DoctorUerId);
            if (doctor == null)
            {
                throw new Exception("dont found this doctor");
            }

            return await context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p.user)
                .Include(a => a.Doctor).ThenInclude(d => d.user)
                .Where(a => a.DoctorId == doctor.Id)
                .OrderBy(a => a.AppointmentDate)
                .Select(a => new AppointmentDto
                {
                    ID = a.ID,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.user.FullName,
                    DoctorId = a.Doctor.Id,
                    DoctorName = a.Doctor.user.FullName,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.status.ToString(),
                    Notes = a.Notes



                }).ToListAsync();

        }

        public async Task<bool> CancelAppointAsync(Guid AppointmentId, Guid UserId)
        {
            var appointment = await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.ID == AppointmentId);


            if (appointment == null)
            {
                throw new Exception("appointment not found");
            }
            if (appointment.Patient.UserID != UserId&& appointment.Doctor.UserID != UserId)
            {
                throw new Exception("you can`t do it");
            }
            appointment.status = AppointmentStatus.Cancelled;
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> AddDoctorScheduleAsync(Guid DoctorUserId, CreateScheduleDto dto)
        {
            var doctor = await context.Doctors.FirstOrDefaultAsync(d=>d.UserID == DoctorUserId);
            if (doctor == null)
            {

                throw new KeyNotFoundException("no Doctor record found associated with this user id");
            }
            var schedule = new DoctorSchedule
            {
                Id = Guid.NewGuid(),
                DoctorId = doctor.Id,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                SlotDurationMinutes = dto.SlotDurationMinutes
            };
            context.DoctorSchedules.Add(schedule);
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message;
                throw new Exception($"Database Error :{innerMessage}");
            }
        }

        public async Task<IEnumerable<CreateScheduleDto>> GetDoctorSchedulesAsync(Guid doctorId)
        {

            return await context.DoctorSchedules.Where(s => s.DoctorId == doctorId).Select(s => new CreateScheduleDto
            {
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                SlotDurationMinutes = s.SlotDurationMinutes
            }).ToListAsync();
                }

       public async Task<IEnumerable<AvailablesDto>> GetAvailableSlotsAsync(Guid doctorId, DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            var schedule = await context.DoctorSchedules.FirstOrDefaultAsync(d=>d.DoctorId==doctorId&&d.DayOfWeek==dayOfWeek);
            if(schedule == null)
            {
                return Enumerable.Empty<AvailablesDto>();
            }
            var bookedTime = await context.Appointments.Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date && a.status != AppointmentStatus.Cancelled)
                .Select(a => a.AppointmentDate.TimeOfDay).ToListAsync();
            var solts =new List<AvailablesDto>();
            var currentTime = schedule.StartTime;
            while(currentTime.Add(TimeSpan.FromMinutes(schedule.SlotDurationMinutes))<=schedule.EndTime)
            {
                var slotdatetime = date.Date.Add(currentTime);
                bool IsBooked = bookedTime.Contains(currentTime);

                solts.Add(new AvailablesDto
                {
                    SlotDateTime = slotdatetime,
                    IsAvailable = !IsBooked,
                });
                currentTime = currentTime.Add(TimeSpan.FromMinutes(schedule.SlotDurationMinutes));
            }
            return solts;

        }
    }
}
