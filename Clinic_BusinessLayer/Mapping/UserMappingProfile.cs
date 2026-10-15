using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Auth;
using Clinic_BusinessLayer.DTOs.Requests.Users;
using Clinic_BusinessLayer.DTOs.Responses.Users;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Entity → Response
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src =>
                        $"{src.Person.FirstName} {src.Person.MiddleName} {src.Person.LastName}".Replace("  ", " ").Trim()))
                .ForMember(dest => dest.RoleName,
                    opt => opt.MapFrom(src => src.Role.Name));

            // Request → Entity
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Person, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.AuditLogs, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());   // ← الـ Service هيعمله Hash

            CreateMap<UpdateUserRequest, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Request → Person
            CreateMap<CreateUserRequest, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

            CreateMap<UpdateUserRequest, Person>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));



            // 🆕 RegisterRequest → Person
            CreateMap<RegisterRequest, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        }
    }
}
