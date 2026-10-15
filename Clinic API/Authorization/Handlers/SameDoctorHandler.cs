using Clinic_API.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Clinic_API.Authorization.Handlers
{
    /// <summary>
    /// بيعمل الـ Logic الفعلي للـ SameDoctorRequirement.
    /// بيتحقق إن:
    /// - الـ User ADMIN → مسموح دايمًا
    /// - الـ User DOCTOR → لازم يكون نفس الدكتور
    /// - أي Role تاني → مرفوض
    /// </summary>
    public class SameDoctorHandler : AuthorizationHandler<SameDoctorRequirement, int>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SameDoctorRequirement requirement,
            int resourceDoctorId)
        {
            // ============ 1. لو ADMIN → اقبل على طول ============
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // ============ 2. لو DOCTOR → تحقق من الـ DoctorId ============
            var userDoctorIdClaim = context.User.FindFirst("DoctorId")?.Value;

            if (!string.IsNullOrEmpty(userDoctorIdClaim) &&
                int.TryParse(userDoctorIdClaim, out var userDoctorId) &&
                userDoctorId == resourceDoctorId)
            {
                context.Succeed(requirement);
            }

            // ============ 3. غير كده → Forbidden ============
            // مش بنعمل حاجة → ASP.NET Core هيرجع 403 Forbidden تلقائي
            return Task.CompletedTask;
        }
    }
}