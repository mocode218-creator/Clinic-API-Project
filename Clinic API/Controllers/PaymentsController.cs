using Clinic_BusinessLayer.DTOs.Requests.Payments;
using Clinic_BusinessLayer.DTOs.Responses.Payments;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Payments")]
    [Produces("application/json")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;
        private readonly IValidator<CreatePaymentRequest> _createValidator;

        public PaymentsController(
            IPaymentService service,
            IValidator<CreatePaymentRequest> createValidator)
        {
            _service = service;
            _createValidator = createValidator;
        }

        [HttpGet("All", Name = "GetAllPayments")]
        [ProducesResponseType(typeof(IEnumerable<PaymentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetPaymentById")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("ByInvoice/{invoiceId}", Name = "GetPaymentsByInvoice")]
        [ProducesResponseType(typeof(IEnumerable<PaymentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByInvoice(int invoiceId)
        {
            var result = await _service.GetByInvoiceAsync(invoiceId);
            return Ok(result);
        }

        [HttpPost(Name = "CreatePayment")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
        {
            var validation = await _createValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // ⚠️ مفيش Update / Delete — الفلوس مبتتعدلش
    }
}