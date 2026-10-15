using Clinic_API.Authorization.Handlers;
using Clinic_API.Authorization.Requirements;
using Clinic_API.Services;
using Clinic_BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;                     // ← 🆕
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;                        // ← 🆕

namespace Clinic_API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // 2. CORS
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                {
                    var allowedOrigins = configuration
                        .GetSection("Cors:AllowedOrigins")
                        .Get<string[]>() ?? Array.Empty<string>();

                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            // 3. Authorization Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SameDoctor", policy =>
                    policy.Requirements.Add(new SameDoctorRequirement()));

                options.AddPolicy("SameUser", policy =>
                    policy.Requirements.Add(new SameUserRequirement()));
            });

            // 4. Authorization Handlers
            services.AddSingleton<IAuthorizationHandler, SameDoctorHandler>();
            services.AddSingleton<IAuthorizationHandler, SameUserHandler>();

            // 5. IHttpContextAccessor
            services.AddHttpContextAccessor();

            // 6. CurrentUserService
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // 7. ✅ Rate Limiting
            services.AddRateLimiter(options =>
            {
                // ✅ لما الحد يتعدى → 429
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // ✅ Global Policy — للـ Authenticated Users
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                    httpContext =>
                    {
                        // لو مسجل دخول → قسّم بالـ UserId
                        var userId = httpContext.User.FindFirst(
                            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                        if (!string.IsNullOrEmpty(userId))
                        {
                            return RateLimitPartition.GetFixedWindowLimiter(
                                partitionKey: $"user_{userId}",
                                factory: _ => new FixedWindowRateLimiterOptions
                                {
                                    PermitLimit = 100,                  // 100 Requests
                                    Window = TimeSpan.FromMinutes(1),   // في الدقيقة
                                    QueueLimit = 0
                                });
                        }

                        // لو مش مسجل → قسّم بالـ IP
                        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                        return RateLimitPartition.GetFixedWindowLimiter(
                            partitionKey: $"ip_{ipAddress}",
                            factory: _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 30,                    // 30 Requests
                                Window = TimeSpan.FromMinutes(1),    // في الدقيقة
                                QueueLimit = 0
                            });
                    });

                // ✅ Policy للـ Login — صارم جدًا
                options.AddFixedWindowLimiter("LoginPolicy", opt =>
                {
                    opt.PermitLimit = 5;                             // 5 محاولات
                    opt.Window = TimeSpan.FromMinutes(1);            // في الدقيقة
                    opt.QueueLimit = 0;
                });

                // ✅ Policy للـ Register — صارم
                options.AddFixedWindowLimiter("RegisterPolicy", opt =>
                {
                    opt.PermitLimit = 3;
                    opt.Window = TimeSpan.FromMinutes(5);
                    opt.QueueLimit = 0;
                });

                // ✅ Policy للـ Refresh Token
                options.AddFixedWindowLimiter("RefreshPolicy", opt =>
                {
                    opt.PermitLimit = 10;
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.QueueLimit = 0;
                });
            });

            return services;
        }
    }
}