using Clinic_BusinessLayer.DTOs.Requests.Doctors;
using Clinic_BusinessLayer.DTOs.Responses.Doctors;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Doctors")]
    [Produces("application/json")]
    public class DoctorsController : ControllerBase
    {

        private readonly IDoctorService _service;
        private readonly IValidator<CreateDoctorRequest> _createValidator;
        private readonly IValidator<UpdateDoctorRequest> _updateValidator;

        public DoctorsController(
            IDoctorService service,
            IValidator<CreateDoctorRequest> createValidator,
            IValidator<UpdateDoctorRequest> updateValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet("All", Name = "GetAllDoctors")]
        [ProducesResponseType(typeof(IEnumerable<DoctorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetDoctorById")]
        [ProducesResponseType(typeof(DoctorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"Doctor with id {id} not found" });

            return Ok(result);
        }

        [HttpGet("BySpecialization/{specializationId}", Name = "GetDoctorsBySpecialization")]
        [ProducesResponseType(typeof(IEnumerable<DoctorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBySpecialization(int specializationId)
        {
            var result = await _service.GetBySpecializationAsync(specializationId);
            return Ok(result);
        }

        [HttpPost(Name = "CreateDoctor")]
        [ProducesResponseType(typeof(DoctorResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateDoctorRequest request)
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

        [HttpPut("{id}", Name = "UpdateDoctor")]
        [ProducesResponseType(typeof(DoctorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorRequest request)
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

        [HttpDelete("{id}", Name = "DeleteDoctor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Doctor with id {id} not found" });

            return NoContent();
        }

    }
}
