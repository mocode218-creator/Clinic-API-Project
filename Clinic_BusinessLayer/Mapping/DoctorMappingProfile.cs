using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Doctors;
using Clinic_BusinessLayer.DTOs.Responses.Doctors;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class DoctorMappingProfile : Profile
    {
        public DoctorMappingProfile()
        {
            // Entity → Response
            CreateMap<Doctor, DoctorResponse>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src =>
                        $"{src.Person.FirstName} {src.Person.MiddleName} {src.Person.LastName}".Replace("  ", " ").Trim()))
                .ForMember(dest => dest.Phone,
                    opt => opt.MapFrom(src => src.Person.Phone))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Person.Email))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Person.Gender))
                .ForMember(dest => dest.SpecializationName,
                    opt => opt.MapFrom(src => src.Specialization.Name));

            // Request → Entity
            CreateMap<CreateDoctorRequest, Doctor>()
                .ForMember(dest => dest.Person, opt => opt.Ignore())
                .ForMember(dest => dest.Specialization, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.DoctorSchedules, opt => opt.Ignore());

            CreateMap<UpdateDoctorRequest, Doctor>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Request → Person
            CreateMap<CreateDoctorRequest, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

            CreateMap<UpdateDoctorRequest, Person>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}