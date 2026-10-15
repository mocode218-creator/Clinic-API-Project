using Clinic_BusinessLayer.DTOs.Requests.Prescriptions;
using Clinic_BusinessLayer.DTOs.Responses.Prescriptions;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Prescriptions")]
    [Produces("application/json")]
    [Authorize]
    public class PrescriptionsController : ControllerBase
    {

        private readonly IPrescriptionService _service;
        private readonly IValidator<CreatePrescriptionRequest> _createValidator;

        public PrescriptionsController(
            IPrescriptionService service,
            IValidator<CreatePrescriptionRequest> createValidator)
        {
            _service = service;
            _createValidator = createValidator;
        }

        [HttpGet("All", Name = "GetAllPrescriptions")]
        [ProducesResponseType(typeof(IEnumerable<PrescriptionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetPrescriptionById")]
        [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("ByVisit/{visitId}", Name = "GetPrescriptionsByVisit")]
        [ProducesResponseType(typeof(IEnumerable<PrescriptionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByVisit(int visitId)
        {
            var result = await _service.GetByVisitAsync(visitId);
            return Ok(result);
        }

        [HttpPost(Name = "CreatePrescription")]
        [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}", Name = "UpdatePrescription")]
        [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePrescriptionRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        // ⚠️ مفيش Delete — Prescriptions immutable

    }
}
