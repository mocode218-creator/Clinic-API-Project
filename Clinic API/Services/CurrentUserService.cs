using Clinic_BusinessLayer.Interfaces;
using System.Security.Claims;

namespace Clinic_API.Services
{
    /// <summary>
    /// Implementation للـ ICurrentUserService.
    /// بيقرأ معلومات المستخدم من HttpContext.User (الـ Claims).
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// بيسهّل الوصول للـ User الحالي
        /// </summary>
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int? UserId =>
            int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
                ? id
                : null;

        public string? UserName =>
            User?.FindFirst(ClaimTypes.Name)?.Value;

        public string? Role =>
            User?.FindFirst(ClaimTypes.Role)?.Value;

        public int? PersonId =>
            int.TryParse(User?.FindFirst("PersonId")?.Value, out var id)
                ? id
                : null;

        public int? DoctorId =>
            int.TryParse(User?.FindFirst("DoctorId")?.Value, out var id)
                ? id
                : null;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role) =>
            User?.IsInRole(role) ?? false;
    }
}