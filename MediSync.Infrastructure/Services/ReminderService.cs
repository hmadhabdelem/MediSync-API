using MediSync.Application.DTOs.Reminders;
using MediSync.Application.Interfaces;
using MediSync.Domain.Entities;
using MediSync.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Infrastructure.Services
{
    public class ReminderService :IReminderService
    {
        private readonly ApplicationDbContext context;
        public ReminderService(ApplicationDbContext _context)
        {
            context= _context;
        }

        public async Task<bool> CreateReminderAsync(Guid userid, CreateReminderDto dto)
        {
           
            var reminder = new Reminder
            {
                id = Guid.NewGuid(),
                Userid = userid,
                PrescriptionItemId = dto.PrescriptionITemID,
                ScheduledTime = dto.ScheduleTime,
                status = Domain.Enums.ReminderStatus.Pending
            };
            context.Reminders.Add(reminder);
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<ReminderDto>> GetPendingRemindersAsync(Guid patientUserId)
        {
            var patient = await context.Patients.FirstOrDefaultAsync(p => p.UserID == patientUserId);
            if (patient == null) { throw new Exception("patient not found"); }

            return await context.Reminders.Include(r=>r.PrescriptionItem).ThenInclude(pi=>pi.Prescription).Where(r=>r.PrescriptionItem.Prescription.PatientID==patient.Id &&r.status==Domain.Enums.ReminderStatus.Pending)
                .OrderBy(r=>r.ScheduledTime).Select(r=>new ReminderDto
                {
                   ID=r.id,
                   PrescriptionITemID=r.PrescriptionItemId,
                   MedicationName=r.PrescriptionItem.MedicineName,
                   ScheduleTime=r.ScheduledTime,
                   Status =r.status.ToString()





                }).ToListAsync();
                
        }

        public async Task<bool> MarkAsReadAsync(Guid patientUserId, Guid reminderId)
        {
            var reminder = await context.Reminders.FirstOrDefaultAsync(r => r.id == reminderId);
            if (reminder == null) { throw new Exception("Reminder not found"); }

            reminder.status = Domain.Enums.ReminderStatus.Taken;
            reminder.TakenAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
