using Clinic_BusinessLayer.DTOs.Responses.AuditLogs;
using Clinic_BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/AuditLogs")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class AuditLogsController : ControllerBase
    {

        private readonly IAuditLogService _service;

        public AuditLogsController(IAuditLogService service)
        {
            _service = service;
        }

        [HttpGet("All", Name = "GetAllAuditLogs")]
        [ProducesResponseType(typeof(IEnumerable<AuditLogResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:long}", Name = "GetAuditLogById")]
        [ProducesResponseType(typeof(AuditLogResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("ByRecord", Name = "GetAuditLogsByRecord")]
        [ProducesResponseType(typeof(IEnumerable<AuditLogResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByRecord([FromQuery] string tableName, [FromQuery] long recordId)
        {
            var result = await _service.GetByRecordAsync(tableName, recordId);
            return Ok(result);
        }

        [HttpGet("ByUser/{userId}", Name = "GetAuditLogsByUser")]
        [ProducesResponseType(typeof(IEnumerable<AuditLogResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var result = await _service.GetByUserAsync(userId);
            return Ok(result);
        }

        // ⚠️ AuditLog للتسجيل بس — مفيش Create/Update/Delete من المستخدم

    }
}
