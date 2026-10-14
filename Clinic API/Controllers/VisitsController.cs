using Clinic_BusinessLayer.DTOs.Requests.Visits;
using Clinic_BusinessLayer.DTOs.Responses.Visits;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Visits")]
    [Produces("application/json")]
    public class VisitsController : ControllerBase
    {

        private readonly IVisitService _service;
        private readonly IValidator<CreateVisitRequest> _createValidator;
        private readonly IValidator<UpdateVisitRequest> _updateValidator;

        public VisitsController(
            IVisitService service,
            IValidator<CreateVisitRequest> createValidator,
            IValidator<UpdateVisitRequest> updateValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet("All", Name = "GetAllVisits")]
        [ProducesResponseType(typeof(IEnumerable<VisitResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetVisitById")]
        [ProducesResponseType(typeof(VisitResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"Visit with id {id} not found" });

            return Ok(result);
        }

        [HttpGet("ByAppointment/{appointmentId}", Name = "GetVisitByAppointment")]
        [ProducesResponseType(typeof(VisitResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByAppointment(int appointmentId)
        {
            var result = await _service.GetByAppointmentAsync(appointmentId);
            if (result == null)
                return NotFound(new { message = $"No visit found for appointment {appointmentId}" });

            return Ok(result);
        }

        [HttpPost(Name = "CreateVisit")]
        [ProducesResponseType(typeof(VisitResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateVisitRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            try
            {
                var result = await _service.CreateAsync(request);
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

        [HttpPut("{id}", Name = "UpdateVisit")]
        [ProducesResponseType(typeof(VisitResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVisitRequest request)
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
        }

        [HttpDelete("{id}", Name = "DeleteVisit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Visit with id {id} not found" });

            return NoContent();
        }

    }
}
