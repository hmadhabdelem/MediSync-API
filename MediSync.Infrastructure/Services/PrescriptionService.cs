using MediSync.Application.DTOs.Prescriptions;
using MediSync.Application.Interfaces;
using MediSync.Domain.Entities;
using MediSync.Domain.Enums;
using MediSync.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Infrastructure.Services
{
    public class PrescriptionService : IPrescriptionService

    {
         private readonly ApplicationDbContext context;
        public PrescriptionService(ApplicationDbContext _context)
        {
            context = _context;
            
        }
        public async Task<PrescriptionDto> CreatePrescriptionAsync(Guid doctorUserId, CreatePrescriptionDto dto)
        {
            var doctor = await context.Doctors.FirstOrDefaultAsync(d=>d.UserID == doctorUserId);

            if (doctor == null) 
            { throw new Exception("doctor not found"); }


                var appointment = await context.Appointments
                    .Include(a => a.Patient).ThenInclude(p => p.user)
                    .Include(a => a.Doctor).ThenInclude(d => d.user)
                    .FirstOrDefaultAsync(a => a.ID == dto.AppointmentId);

            if (appointment == null)
            {
                throw new Exception("appointment not found");
            }
            if (appointment.DoctorId!= doctor.Id)
            {
                throw new Exception("not authentaction to write a prescription ");
            }

            var prescription = new Prescription
            {
                id = Guid.NewGuid(),
                PatientID = appointment.PatientId,
                DoctorId = doctor.Id,
                Status = PrescriptionStatus.Active,
                Diagnosis=dto.Diagnosis,
                CreatedAt = DateTime.UtcNow,
                Items = dto.Item.Select(i => new PrescriptionItem
                {
                    id = Guid.NewGuid(),
                    MedicineName = i.MedicationName,
                    Dosage = i.Dosage,
                    FrequencyPerDay = i.Frequency,
                    DurationDays = i.Duration

                }).ToList()
            };
            
            context.Prescriptions.Add(prescription);
            await context.SaveChangesAsync();
            return new PrescriptionDto
            {
                Id = prescription.id,
                PatientNAme = appointment.Patient?.user?.FullName ?? "",
                DoctorName = appointment.Doctor.user?.FullName ?? "",
                Diagnosis = prescription.Diagnosis,
                CreatedAt = prescription.CreatedAt,
                Items = prescription.Items.Select(i => new PrescriptionItemDto
                {
                    MedicationName = i.MedicineName,
                    Dosage = i.Dosage,
                    Frequency = i.FrequencyPerDay,
                    Duration = i.DurationDays

                }).ToList()


            };
        }

        public async Task<IEnumerable<PrescriptionDto>> GetPatientPrescriptionsAsync(Guid patientUserId)
        {
            var patient = await context.Patients.FirstOrDefaultAsync(a => a.UserID == patientUserId);
            if (patient == null) { throw new Exception("patient not found"); }
            return await context.Prescriptions
                .Include(p => p.Patient).ThenInclude(p => p.user)
                .Include(d => d.Doctor).ThenInclude(D => D.user)
                .Include(i => i.Items)
                .Where(p => p.PatientID == patient.Id)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PrescriptionDto
                {
                    Id = p.id,
                    PatientNAme = p.Patient.user.FullName,
                    DoctorName = p.Doctor.user.FullName,
                    Diagnosis = p.Diagnosis,
                    CreatedAt = p.CreatedAt,
                    Items = p.Items.Select(i => new PrescriptionItemDto
                    {

                        MedicationName = i.MedicineName,
                        Dosage = i.Dosage,
                        Frequency = i.FrequencyPerDay,
                        Duration = i.DurationDays

                    }).ToList()

                }).ToListAsync();


                } 


        public async Task<PrescriptionDto?> GetPrescriptionByAppointmentIdAsync(Guid appointmentId)
        {
            var appointment = await context.Appointments.FirstOrDefaultAsync(a=>a.ID == appointmentId);
            if (appointment == null) {return null;}
            return await context.Prescriptions
                .Include(p => p.Patient).ThenInclude(p => p.user)
                .Include(d => d.Doctor).ThenInclude(D => D.user)
                .Include(i => i.Items)
                .Where(p => p.PatientID==appointment.PatientId && p.DoctorId == appointment.DoctorId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PrescriptionDto
                {
                    Id = p.id,
                    PatientNAme = p.Patient.user.FullName,
                    DoctorName = p.Doctor.user.FullName,
                    Diagnosis = p.Diagnosis,
                    CreatedAt = p.CreatedAt,
                    Items = p.Items.Select(i => new PrescriptionItemDto
                    {

                        MedicationName = i.MedicineName,
                        Dosage = i.Dosage,
                        Frequency = i.FrequencyPerDay,
                        Duration = i.DurationDays

                    }).ToList()
                }).FirstOrDefaultAsync();   
        }
    }
}
