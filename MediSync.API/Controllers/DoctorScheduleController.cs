using MediSync.Application.DTOs.Appointments;
using MediSync.Application.Interfaces;
using MediSync.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorScheduleController : ControllerBase
    {

        private readonly IAppointmentService appointmentService;
        public DoctorScheduleController(IAppointmentService appointment)
        {
            appointmentService = appointment;
        }

        [HttpPost("Schedules")]
        [Authorize(Roles = "Doctor")]

        public async Task<IActionResult> AddSchedule(CreateScheduleDto dto)
        {
            
                var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await appointmentService.AddDoctorScheduleAsync(userid, dto);
                return Ok(new { message = "done adding" });
            

        }

        [HttpGet("Schedule/{doctorid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorSchedules(Guid doctorid)
        {
            var schedules = await appointmentService.GetDoctorSchedulesAsync(doctorid);
            return Ok(schedules);
        }

        [HttpGet("available-slots/{doctorid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableSlotsAsync(Guid doctorid, DateTime date)
        {
            var slots = await appointmentService.GetAvailableSlotsAsync(doctorid, date);
            return Ok(slots);
        }

    }
}
