using Clinic_BusinessLayer.DTOs.Requests.Appointments;
using Clinic_BusinessLayer.DTOs.Responses.Appointments;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Appointments")]
    [Produces("application/json")]
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
            if (result == null)
                return NotFound(new { message = $"Appointment with id {id} not found" });

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
            var createdByUserId = int.TryParse(userIdClaim, out var uid) ? uid : 1;

            try
            {
                var result = await _service.CreateAsync(request, createdByUserId);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
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

            try
            {
                var result = await _service.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/Cancel", Name = "CancelAppointment")]
        [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var result = await _service.CancelAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}", Name = "DeleteAppointment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Appointment with id {id} not found" });

            return NoContent();
        }

    }
}
