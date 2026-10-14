using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Users;
using Clinic_BusinessLayer.DTOs.Responses.Users;
using Clinic_BusinessLayer.Helpers;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetWithPersonAsync(id);
            if (user == null || user.DeletedAt != null)
                return null;

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            // 1. التحقق من اسم المستخدم
            var existing = await _unitOfWork.Users.IsUsernameExistsAsync(request.UserName);
            if (existing)
                throw new InvalidOperationException($"Username '{request.UserName}' already exists");

            // 2. التحقق من الـ Role
            var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);
            if (role == null || role.DeletedAt != null)
                throw new KeyNotFoundException($"Role with id {request.RoleId} not found");

            // 3. إنشاء Person
            var person = _mapper.Map<Person>(request);
            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.SaveChangesAsync();

            // 4. إنشاء User
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
                throw new KeyNotFoundException($"User with id {id} not found");

            // التحقق من اسم المستخدم لو اتغير
            if (user.UserName != request.UserName)
            {
                var existing = await _unitOfWork.Users.IsUsernameExistsAsync(request.UserName, id);
                if (existing)
                    throw new InvalidOperationException($"Username '{request.UserName}' already exists");
            }

            // تعديل الـ Person
            _mapper.Map(request, user.Person);
            _unitOfWork.Persons.Update(user.Person);

            // تعديل الـ User
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
                return false;

            user.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
