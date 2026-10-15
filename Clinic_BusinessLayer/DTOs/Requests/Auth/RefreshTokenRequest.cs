namespace Clinic_BusinessLayer.DTOs.Requests.Auth
{
    public class RefreshTokenRequest
    {
        /// <summary>
        /// الـ Refresh Token اللي المستخدم استلمه مع الـ Login.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}