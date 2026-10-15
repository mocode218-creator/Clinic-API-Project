using Clinic_BusinessLayer.DTOs.Requests.Branches;
using Clinic_BusinessLayer.DTOs.Responses.Branches;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Branches")]
    [Produces("application/json")]
    [Authorize]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _service;
        private readonly IValidator<CreateBranchRequest> _createValidator;
        private readonly IValidator<UpdateBranchRequest> _updateValidator;

        public BranchesController(
            IBranchService service,
            IValidator<CreateBranchRequest> createValidator,
            IValidator<UpdateBranchRequest> updateValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet("All", Name = "GetAllBranches")]
        [ProducesResponseType(typeof(IEnumerable<BranchResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetBranchById")]
        [ProducesResponseType(typeof(BranchResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost(Name = "CreateBranch")]
        [ProducesResponseType(typeof(BranchResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateBranchRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}", Name = "UpdateBranch")]
        [ProducesResponseType(typeof(BranchResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchRequest request)
        {
            var validation = await _updateValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}", Name = "DeleteBranch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}