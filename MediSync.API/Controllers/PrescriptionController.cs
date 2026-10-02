using MediSync.Application.DTOs.Prescriptions;
using MediSync.Application.Interfaces;
using MediSync.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace MediSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService prescriptionService;
        public PrescriptionController(IPrescriptionService _prescriptionService)
        {
            prescriptionService = _prescriptionService;
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreatePrescription(CreatePrescriptionDto dto)
        {
            
                var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await prescriptionService.CreatePrescriptionAsync(userid, dto);
                return Ok(result);

        }
        [HttpGet("my_Prescription")]
        [Authorize(Roles = "patient")]
        public async Task<IActionResult> GetMyPrescription()
        {
            
                var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await prescriptionService.GetPatientPrescriptionsAsync(userid);
                return Ok(result);

        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IActionResult> GePrescriptionByappointmentID(Guid appointmentId)
        {
            
                var prescription = await prescriptionService.GetPrescriptionByAppointmentIdAsync(appointmentId);
                if (prescription == null)
                {
                    return NotFound(new { message = "not found Prescirption for this appointment" });
                }
                return Ok(prescription);

        }

        }
        }

