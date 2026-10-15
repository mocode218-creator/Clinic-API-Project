using Microsoft.AspNetCore.Authorization;

namespace Clinic_API.Authorization.Requirements
{
    /// <summary>
    /// Requirement بيطلب إن المستخدم الحالي يكون هو نفسه صاحب الـ Resource.
    /// 
    /// الاستخدام:
    /// await _authorizationService.AuthorizeAsync(User, userId, "SameUser")
    /// </summary>
    public class SameUserRequirement : IAuthorizationRequirement
    {
        // مفيش Properties — مجرد "علامة"
    }
}