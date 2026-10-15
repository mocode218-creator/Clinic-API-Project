using Clinic_BusinessLayer.DTOs.Requests.Specializations;
using Clinic_BusinessLayer.DTOs.Responses.Specializations;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Specializations")]
    [Produces("application/json")]
    [Authorize]
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _service;
        private readonly IValidator<CreateSpecializationRequest> _createValidator;
        private readonly IValidator<UpdateSpecializationRequest> _updateValidator;

        public SpecializationsController(
            ISpecializationService service,
            IValidator<CreateSpecializationRequest> createValidator,
            IValidator<UpdateSpecializationRequest> updateValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }


        [HttpGet("All", Name = "GetAllSpecializations")]
        [ProducesResponseType(typeof(IEnumerable<SpecializationResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }


        [HttpGet("{id}", Name = "GetSpecializationById")]
        [ProducesResponseType(typeof(SpecializationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
            // ← مفيش if (null) — الـ Service هيرمي Exception
        }



        [HttpPost(Name = "CreateSpecialization")]
        [ProducesResponseType(typeof(SpecializationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateSpecializationRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            // ← مفيش try/catch — الـ Middleware بيمسك
        }


        [HttpPut("{id}", Name = "UpdateSpecialization")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecializationRequest request)
        {
            var validation = await _updateValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}", Name = "DeleteSpecialization")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }


    }
}