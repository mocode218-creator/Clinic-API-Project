namespace Clinic_BusinessLayer.Interfaces
{
    /// <summary>
    /// بيوفّر معلومات عن المستخدم الحالي للـ Services.
    /// 
    /// الـ Services مش عندها وصول لـ HttpContext مباشرة،
    /// فالـ Interface ده بيخليها تعرف مين المستخدم الحالي.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// الـ User ID بتاع المستخدم الحالي
        /// </summary>
        int? UserId { get; }

        /// <summary>
        /// اسم المستخدم الحالي
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// الـ Role بتاع المستخدم الحالي (Admin, Doctor, Receptionist)
        /// </summary>
        string? Role { get; }

        /// <summary>
        /// الـ Person ID المرتبط بالمستخدم
        /// </summary>
        int? PersonId { get; }

        /// <summary>
        /// الـ Doctor ID لو المستخدم دكتور
        /// </summary>
        int? DoctorId { get; }

        /// <summary>
        /// هل المستخدم مسجل دخول؟
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// هل المستخدم عنده Role معين؟
        /// </summary>
        bool IsInRole(string role);
    }
}