namespace Clinic_BusinessLayer.DTOs.Responses.Auth
{
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // Access Token
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }

        // Refresh Token
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; set; }   // ← DateTime مش DateTimeOffset
    }
}