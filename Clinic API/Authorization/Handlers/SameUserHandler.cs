using Clinic_API.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Clinic_API.Authorization.Handlers
{
    /// <summary>
    /// بيعمل الـ Logic الفعلي للـ SameUserRequirement.
    /// 
    /// بيتحقق إن:
    /// - الـ User ADMIN → مسموح دايمًا
    /// - الـ User نفسه → مسموح
    /// - User تاني → 403 Forbidden
    /// </summary>
    public class SameUserHandler : AuthorizationHandler<SameUserRequirement, int>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SameUserRequirement requirement,
            int resourceUserId)
        {
            // ============ 1. Admin → كل حاجة ============
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // ============ 2. User → نفسه بس ============
            var currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(currentUserId) &&
                int.TryParse(currentUserId, out var userId) &&
                userId == resourceUserId)
            {
                context.Succeed(requirement);
            }

            // ============ 3. غير كده → Forbidden ============
            return Task.CompletedTask;
        }
    }
}