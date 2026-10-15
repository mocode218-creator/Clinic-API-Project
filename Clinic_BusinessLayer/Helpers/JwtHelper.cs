using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;   // ← 🆕
using System.Text;

namespace Clinic_BusinessLayer.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// بيولّد الـ Access Token (JWT).
        /// نفس الـ Method القديمة — الاسم ما اتغيرش.
        /// </summary>
        public string GenerateToken(int userId, string userName, string role, int personId, int? doctorId = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role),
                new Claim("PersonId", personId.ToString())
            };

            // ✅ ضيف DoctorId لو المستخدم دكتور
            if (doctorId.HasValue)
            {
                claims.Add(new Claim("DoctorId", doctorId.Value.ToString()));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(
                    double.Parse(_configuration["Jwt:ExpiryHours"] ?? "24")),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// بيولّد Refresh Token (طويل المدى - 7 أيام).
        /// ده Random String مش JWT.
        /// </summary>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// بيرجع تاريخ انتهاء الـ Refresh Token (7 أيام من الآن).
        /// </summary>
        public DateTime GetRefreshTokenExpiry()   // ← DateTime مش DateTimeOffset
        {
            var days = double.Parse(_configuration["Jwt:RefreshTokenExpiryDays"] ?? "7");
            return DateTime.UtcNow.AddDays(days);   // ← DateTime.UtcNow
        }
    }
}