using Clinic_BusinessLayer.DTOs.Requests.Patients;
using Clinic_BusinessLayer.DTOs.Responses.Patients;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Patients")]
    [Produces("application/json")]
    public class PatientsController : ControllerBase
    {

        private readonly IPatientService _service;
        private readonly IValidator<CreatePatientRequest> _createValidator;
        private readonly IValidator<UpdatePatientRequest> _updateValidator;

        public PatientsController(
            IPatientService service,
            IValidator<CreatePatientRequest> createValidator,
            IValidator<UpdatePatientRequest> updateValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet("All", Name = "GetAllPatients")]
        [ProducesResponseType(typeof(IEnumerable<PatientResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetPatientById")]
        [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"Patient with id {id} not found" });

            return Ok(result);
        }

        [HttpGet("Search", Name = "SearchPatients")]
        [ProducesResponseType(typeof(IEnumerable<PatientResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest(new { message = "Search term is required" });

            var result = await _service.SearchAsync(term);
            return Ok(result);
        }

        [HttpPost(Name = "CreatePatient")]
        [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePatientRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}", Name = "UpdatePatient")]
        [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientRequest request)
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

        [HttpDelete("{id}", Name = "DeletePatient")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Patient with id {id} not found" });

            return NoContent();
        }

    }
}
