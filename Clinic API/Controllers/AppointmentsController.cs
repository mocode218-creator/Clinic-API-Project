using Clinic_BusinessLayer.DTOs.Requests.Appointments;
using Clinic_BusinessLayer.DTOs.Responses.Appointments;
using Clinic_BusinessLayer.Exceptions;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Appointments")]
    [Produces("application/json")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {

        private readonly IAppointmentService _service;
        private readonly IValidator<CreateAppointmentRequest> _createValidator;
        private readonly IValidator<UpdateAppointmentRequest> _updateValidator;

        public AppointmentsController(
            IAppointmentService service,
            IValidator<CreateAppointmentRequest> createValidator,
            IValidator<UpdateAppointmentRequest> updateValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet("All", Name = "GetAllAppointments")]
        [ProducesResponseType(typeof(IEnumerable<AppointmentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetAppointmentById")]
        [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("ByPatient/{patientId}", Name = "GetAppointmentsByPatient")]
        [ProducesResponseType(typeof(IEnumerable<AppointmentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await _service.GetByPatientAsync(patientId);
            return Ok(result);
        }

        [HttpGet("ByDoctor/{doctorId}/ByDate", Name = "GetAppointmentsByDoctorAndDate")]
        [ProducesResponseType(typeof(IEnumerable<AppointmentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDoctorAndDate(int doctorId, [FromQuery] DateTime date)
        {
            var result = await _service.GetByDoctorAndDateAsync(doctorId, date);
            return Ok(result);
        }

        [HttpPost(Name = "CreateAppointment")]
        [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedException("User not authenticated");

            var createdByUserId = int.Parse(userIdClaim);

            var result = await _service.CreateAsync(request, createdByUserId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}", Name = "UpdateAppointment")]
        [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentRequest request)
        {
            var validation = await _updateValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpPatch("{id}/Cancel", Name = "CancelAppointment")]
        [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelAsync(id);
            return Ok(result);
        }

        [HttpDelete("{id}", Name = "DeleteAppointment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

    }
}
