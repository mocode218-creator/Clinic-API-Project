using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Auth;
using Clinic_BusinessLayer.DTOs.Responses.Auth;
using Clinic_BusinessLayer.Exceptions;
using Clinic_BusinessLayer.Helpers;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, JwtHelper jwtHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        // ==================== LOGIN ====================
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // 1. التحقق من المستخدم
            var user = await _unitOfWork.Users.GetByUsernameAsync(request.UserName);
            if (user == null)
                throw new UnauthorizedException("Invalid username or password");

            if (!user.IsActive)
                throw new UnauthorizedException("Account is deactivated");

            if (!PasswordHelper.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid username or password");

            // 2. جيب الـ DoctorId لو المستخدم دكتور
            int? doctorId = null;
            if (user.Role.Name == "Doctor" || user.Role.Name == "DOCTOR")
            {
                var doctor = await _unitOfWork.Doctors.GetByPersonIdAsync(user.PersonId);
                doctorId = doctor?.Id;
            }

            // 3. توليد الـ Access Token
            var accessToken = _jwtHelper.GenerateToken(
                user.Id, user.UserName, user.Role.Name, user.PersonId, doctorId);

            // 4. توليد الـ Refresh Token
            var refreshToken = _jwtHelper.GenerateRefreshToken();
            var refreshTokenExpiry = _jwtHelper.GetRefreshTokenExpiry();

            // 5. حفظ الـ Refresh Token في الداتابيز
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = refreshTokenExpiry,
                CreatedAt = DateTime.UtcNow           // ← اتغير
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            // 6. إرجاع الـ Response
            return new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Role = user.Role.Name,
                Token = accessToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiry
            };
        }

        // ==================== REGISTER ====================
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // 1. التحقق من اسم المستخدم
            var existing = await _unitOfWork.Users.IsUsernameExistsAsync(request.UserName);
            if (existing)
                throw new ConflictException($"Username '{request.UserName}' already exists");

            // 2. التحقق من الـ Role
            var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);
            if (role == null || role.DeletedAt != null)
                throw new NotFoundException($"Role with id {request.RoleId} not found");

            // 3. إنشاء Person
            var person = _mapper.Map<Person>(request);
            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.SaveChangesAsync();

            // 4. إنشاء User
            var user = new User
            {
                PersonId = person.Id,
                RoleId = request.RoleId,
                UserName = request.UserName,
                PasswordHash = PasswordHelper.Hash(request.Password),
                IsActive = true
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // 5. جيب الـ DoctorId لو المستخدم دكتور
            int? doctorId = null;
            if (role.Name == "Doctor" || role.Name == "DOCTOR")
            {
                var doctor = await _unitOfWork.Doctors.GetByPersonIdAsync(person.Id);
                doctorId = doctor?.Id;
            }

            // 6. توليد الـ Access Token
            var accessToken = _jwtHelper.GenerateToken(
                user.Id, user.UserName, role.Name, person.Id, doctorId);

            // 7. توليد الـ Refresh Token
            var refreshToken = _jwtHelper.GenerateRefreshToken();
            var refreshTokenExpiry = _jwtHelper.GetRefreshTokenExpiry();

            // 8. حفظ الـ Refresh Token
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = refreshTokenExpiry,
                CreatedAt = DateTime.UtcNow           // ← اتغير
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            // 9. إرجاع الـ Response
            return new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Role = role.Name,
                Token = accessToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiry
            };
        }

        // ==================== REFRESH TOKEN ====================
        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            // 1. دور على الـ Refresh Token في الداتابيز
            var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken);

            if (existingToken == null)
                throw new UnauthorizedException("Invalid refresh token");

            // 2. تحقق إنه مش مبطّل
            if (existingToken.RevokedAt != null)
                throw new UnauthorizedException("Refresh token has been revoked");

            // 3. تحقق إنه مش منتهي
            if (existingToken.ExpiresAt < DateTime.UtcNow)   // ← اتغير
                throw new UnauthorizedException("Refresh token has expired");

            var user = existingToken.User;

            // 4. تحقق إن المستخدم لسه Active
            if (!user.IsActive)
                throw new UnauthorizedException("Account is deactivated");

            // 5. جيب الـ DoctorId لو المستخدم دكتور
            int? doctorId = null;
            if (user.Role.Name == "Doctor" || user.Role.Name == "DOCTOR")
            {
                var doctor = await _unitOfWork.Doctors.GetByPersonIdAsync(user.PersonId);
                doctorId = doctor?.Id;
            }

            // 6. ابطل الـ Refresh Token القديم
            existingToken.RevokedAt = DateTime.UtcNow;   // ← اتغير

            // 7. ولّد Refresh Token جديد
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();
            var newRefreshTokenExpiry = _jwtHelper.GetRefreshTokenExpiry();

            existingToken.ReplacedByToken = newRefreshToken;

            _unitOfWork.RefreshTokens.Update(existingToken);

            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = newRefreshTokenExpiry,
                CreatedAt = DateTime.UtcNow           // ← اتغير
            };

            await _unitOfWork.RefreshTokens.AddAsync(newRefreshTokenEntity);

            // 8. ولّد Access Token جديد
            var newAccessToken = _jwtHelper.GenerateToken(
                user.Id, user.UserName, user.Role.Name, user.PersonId, doctorId);

            await _unitOfWork.SaveChangesAsync();

            // 9. إرجاع الـ Response
            return new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Role = user.Role.Name,
                Token = newAccessToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                RefreshToken = newRefreshToken,
                RefreshTokenExpiresAt = newRefreshTokenExpiry
            };
        }

        // ==================== LOGOUT ====================
        public async Task LogoutAsync(LogoutRequest request)
        {
            // 1. دور على الـ Refresh Token
            var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken);

            if (existingToken == null)
                throw new UnauthorizedException("Invalid refresh token");

            // 2. تحقق إنه مش مبطّل من قبل
            if (existingToken.RevokedAt != null)
                throw new UnauthorizedException("Refresh token has been revoked");

            // 3. ابطله
            existingToken.RevokedAt = DateTime.UtcNow;   // ← اتغير
            _unitOfWork.RefreshTokens.Update(existingToken);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}