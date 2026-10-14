using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Patients;
using Clinic_BusinessLayer.DTOs.Responses.Patients;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class PatientMappingProfile : Profile
    {
        public PatientMappingProfile()
        {
            // Entity → Response
            CreateMap<Patient, PatientResponse>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src =>
                        $"{src.Person.FirstName} {src.Person.MiddleName} {src.Person.LastName}".Replace("  ", " ").Trim()))
                .ForMember(dest => dest.Phone,
                    opt => opt.MapFrom(src => src.Person.Phone))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Person.Email))
                .ForMember(dest => dest.BirthDate,
                    opt => opt.MapFrom(src => src.Person.BirthDate))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Person.Gender));

            // Request → Entity (Person + Patient)
            // الـ Service هيعمل Person الأول، وبعدين Patient
            CreateMap<CreatePatientRequest, Patient>()
                .ForMember(dest => dest.Person, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            CreateMap<UpdatePatientRequest, Patient>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Request → Person
            CreateMap<CreatePatientRequest, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

            CreateMap<UpdatePatientRequest, Person>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
