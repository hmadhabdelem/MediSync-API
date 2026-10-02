using MediSync.Application.DTOs.Appointments;
using MediSync.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
       private readonly  IAppointmentService appointmentService;
        public AppointmentsController(IAppointmentService appointment)
        {
            appointmentService = appointment;
        }

        [HttpPost("book")]
        [Authorize(Roles ="Patient")]
        public async Task<IActionResult> BookAppointment(CreateAppointmentDto dto)
        {
            
            
                var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await appointmentService.BookAppointmentAsync(userid, dto);
                return Ok(result);
        }


        [HttpGet("my appointments")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments()
        {
            
                var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await appointmentService.GetPatientAppointmentsAsync(userid);
                return Ok(result);

        }

        [HttpGet("Doctor appointments")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            
                var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await appointmentService.GetDoctorAppointmentsAsync(userid);
                return Ok(result);

            

        }

        [HttpPut("Cancel/{Id}")]
        public async Task<IActionResult> CancelAppointment(Guid Id)
        {
            
                var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await appointmentService.CancelAppointAsync(Id, userid);
                return Ok(new { message = "done Cancel your Appointment" });
        }



    }
}
