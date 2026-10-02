using MediSync.Application.DTOs.Reminders;
using MediSync.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class RemindersController : ControllerBase
    {
        private readonly IReminderService reminderService;
        public RemindersController(IReminderService _reminderservice)
        {
            reminderService = _reminderservice;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReminder(CreateReminderDto dto)
        {
            var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            await reminderService.CreateReminderAsync(userid, dto);
            return Ok(new { message = "Done ,Reminder is Created Successed" });

        }
        [HttpGet("Pending")]
        public async Task<IActionResult> GetPendingReminders()
        {
            var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var reminders = await reminderService.GetPendingRemindersAsync(userid);
            return Ok(reminders);
        }
        [HttpPut("{id}/Taken")]
        public async Task<IActionResult> MarkAsTaken(Guid id)
        {
            var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await reminderService.MarkAsReadAsync(userid, id);
            return Ok(new { message = "Reminder is Updated" });

        }
    }
    }
