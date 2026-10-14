using Clinic_BusinessLayer.DTOs.Responses.Invoices;
using Clinic_BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Invoices")]
    [Produces("application/json")]
    public class InvoicesController : ControllerBase
    {

        private readonly IInvoiceService _service;

        public InvoicesController(IInvoiceService service)
        {
            _service = service;
        }

        [HttpGet("All", Name = "GetAllInvoices")]
        [ProducesResponseType(typeof(IEnumerable<InvoiceResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetInvoiceById")]
        [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"Invoice with id {id} not found" });

            return Ok(result);
        }

        [HttpGet("ByVisit/{visitId}", Name = "GetInvoiceByVisit")]
        [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByVisit(int visitId)
        {
            var result = await _service.GetByVisitAsync(visitId);
            if (result == null)
                return NotFound(new { message = $"No invoice found for visit {visitId}" });

            return Ok(result);
        }

        [HttpPost("Generate/{visitId}", Name = "GenerateInvoice")]
        [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Generate(int visitId)
        {
            try
            {
                var result = await _service.GenerateAsync(visitId);
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

        [HttpPatch("{id}/Status", Name = "UpdateInvoiceStatus")]
        [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            try
            {
                var result = await _service.UpdateStatusAsync(id, status);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ⚠️ مفيش Delete — الفواتير مبتتمسحش

    }
}
