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

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(request.UserName);
            if (user == null)
                throw new UnauthorizedException("Invalid username or password");

            if (!user.IsActive)
                throw new UnauthorizedException("Account is deactivated");

            if (!PasswordHelper.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid username or password");

            var token = _jwtHelper.GenerateToken(
                user.Id, user.UserName, user.Role.Name, user.PersonId);

            return new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Role = user.Role.Name,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existing = await _unitOfWork.Users.IsUsernameExistsAsync(request.UserName);
            if (existing)
                throw new ConflictException($"Username '{request.UserName}' already exists");

            var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);
            if (role == null || role.DeletedAt != null)
                throw new NotFoundException($"Role with id {request.RoleId} not found");

            var person = _mapper.Map<Person>(request);
            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.SaveChangesAsync();

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

            var token = _jwtHelper.GenerateToken(
                user.Id, user.UserName, role.Name, person.Id);

            return new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Role = role.Name,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }
    }
}