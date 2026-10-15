using Clinic_BusinessLayer.DTOs.Requests.Auth;
using Clinic_BusinessLayer.DTOs.Responses.Auth;

namespace Clinic_BusinessLayer.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);   // ← 🆕
        Task LogoutAsync(LogoutRequest request);                             // ← 🆕
    }
}