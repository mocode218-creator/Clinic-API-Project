using Clinic_DataAccess.Entities;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        /// <summary>
        /// بيدور على Refresh Token معين
        /// </summary>
        Task<RefreshToken?> GetByTokenAsync(string token);

        /// <summary>
        /// بيجيب كل الـ Active Tokens لمستخدم معين
        /// </summary>
        Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(int userId);

        /// <summary>
        /// بيلغي كل الـ Tokens بتاعة مستخدم (للـ Logout من كل الأجهزة)
        /// </summary>
        Task RevokeAllUserTokensAsync(int userId);
    }
}