namespace Clinic_BusinessLayer.DTOs.Requests.Auth
{
    public class LogoutRequest
    {
        /// <summary>
        /// الـ Refresh Token اللي عايز نبطّله.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}