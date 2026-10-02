using MediSync.Application.DTOs.Reminders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediSync.Application.Interfaces
{
    public interface IReminderService
    {
        Task<bool> CreateReminderAsync(Guid userid, CreateReminderDto dto);
        Task<IEnumerable<ReminderDto>>GetPendingRemindersAsync(Guid patientUserId);
        Task<bool> MarkAsReadAsync(Guid patientUserId, Guid reminderId);
    }
}
