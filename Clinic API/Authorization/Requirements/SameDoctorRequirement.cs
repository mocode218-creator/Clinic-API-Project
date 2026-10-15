using Microsoft.AspNetCore.Authorization;

namespace Clinic_API.Authorization.Requirements
{
    /// <summary>
    /// Requirement بيطلب إن المستخدم الحالي يكون هو نفس الدكتور المرتبط بالـ Resource.
    /// 
    /// الاستخدام:
    /// [Authorize(Policy = "SameDoctor")] أو
    /// _authorizationService.AuthorizeAsync(User, resourceId, "SameDoctor")
    /// </summary>
    public class SameDoctorRequirement : IAuthorizationRequirement
    {
        // Requirement مجرد "علامة" — مفيش بيانات.
        // الـ Handler هو اللي بيعمل المنطق الفعلي.
    }
}