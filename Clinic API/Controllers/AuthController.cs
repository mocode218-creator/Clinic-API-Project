using Clinic_BusinessLayer.DTOs.Requests.Auth;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Clinic_API.Controllers
{
    [ApiController]
    [Route("api/v1/Auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly IValidator<RefreshTokenRequest> _refreshValidator;   // ← 🆕

        public AuthController(
            IAuthService service,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterRequest> registerValidator,
            IValidator<RefreshTokenRequest> refreshValidator)                 // ← 🆕
        {
            _service = service;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _refreshValidator = refreshValidator;                             // ← 🆕
        }

        [HttpPost("Login", Name = "Login")]
        [AllowAnonymous]
        [EnableRateLimiting("LoginPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var validation = await _loginValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("Register", Name = "Register")]
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("RegisterPolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var validation = await _registerValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.RegisterAsync(request);
            return StatusCode(201, result);
        }

        // ==================== REFRESH TOKEN ====================
        [HttpPost("RefreshToken", Name = "RefreshToken")]
        [AllowAnonymous]                                                    // ← 🆕
        [EnableRateLimiting("RefreshTokenPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var validation = await _refreshValidator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _service.RefreshTokenAsync(request);
            return Ok(result);
        }

        // ==================== LOGOUT ====================
        [HttpPost("Logout", Name = "Logout")]
        [Authorize]                                                         // ← 🆕
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await _service.LogoutAsync(request);
            return NoContent();
        }
    }
}