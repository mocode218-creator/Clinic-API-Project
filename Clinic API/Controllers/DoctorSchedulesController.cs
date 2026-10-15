using Clinic_BusinessLayer.DTOs.Requests.DoctorSchedules;
using Clinic_BusinessLayer.DTOs.Responses.DoctorSchedules;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/DoctorSchedules")]
    [Produces("application/json")]
    public class DoctorSchedulesController : ControllerBase
    {
        private readonly IDoctorScheduleService _service;
        private readonly IValidator<CreateDoctorScheduleRequest> _createValidator;
        private readonly IValidator<UpdateDoctorScheduleRequest> _updateValidator;

        public DoctorSchedulesController(
            IDoctorScheduleService service,
            IValidator<CreateDoctorScheduleRequest> createValidator,
            IValidator<UpdateDoctorScheduleRequest> updateValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet("All", Name = "GetAllDoctorSchedules")]
        [ProducesResponseType(typeof(IEnumerable<DoctorScheduleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetDoctorScheduleById")]
        [ProducesResponseType(typeof(DoctorScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("ByDoctor/{doctorId}", Name = "GetSchedulesByDoctor")]
        [ProducesResponseType(typeof(IEnumerable<DoctorScheduleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var result = await _service.GetByDoctorAsync(doctorId);
            return Ok(result);
        }

        [HttpPost(Name = "CreateDoctorSchedule")]
        [ProducesResponseType(typeof(DoctorScheduleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateDoctorScheduleRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}", Name = "UpdateDoctorSchedule")]
        [ProducesResponseType(typeof(DoctorScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorScheduleRequest request)
        {
            var validation = await _updateValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}", Name = "DeleteDoctorSchedule")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}