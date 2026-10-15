using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Users;
using Clinic_BusinessLayer.DTOs.Responses.Users;
using Clinic_BusinessLayer.Exceptions;                       // ← 🆕
using Clinic_BusinessLayer.Helpers;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetWithPersonAsync(id);
            if (user == null || user.DeletedAt != null)
                throw new NotFoundException($"User with id {id} not found");

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
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

            var user = _mapper.Map<User>(request);
            user.PersonId = person.Id;
            user.PasswordHash = PasswordHelper.Hash(request.Password);
            user.IsActive = true;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            user.Person = person;
            user.Role = role;

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request)
        {
            var user = await _unitOfWork.Users.GetWithPersonAsync(id);
            if (user == null || user.DeletedAt != null)
                throw new NotFoundException($"User with id {id} not found");

            if (user.UserName != request.UserName)
            {
                var existing = await _unitOfWork.Users.IsUsernameExistsAsync(request.UserName, id);
                if (existing)
                    throw new ConflictException($"Username '{request.UserName}' already exists");
            }

            _mapper.Map(request, user.Person);
            _unitOfWork.Persons.Update(user.Person);

            _mapper.Map(request, user);
            _unitOfWork.Users.Update(user);

            await _unitOfWork.SaveChangesAsync();

            var updated = await _unitOfWork.Users.GetWithPersonAsync(id);
            return _mapper.Map<UserResponse>(updated!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null || user.DeletedAt != null)
                throw new NotFoundException($"User with id {id} not found");

            user.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}