using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Appointments;
using Clinic_BusinessLayer.DTOs.Responses.Appointments;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class AppointmentMappingProfile : Profile
    {
        public AppointmentMappingProfile()
        {
            // Entity → Response
            CreateMap<Appointment, AppointmentResponse>()
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src =>
                        $"{src.Patient.Person.FirstName} {src.Patient.Person.LastName}".Trim()))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src =>
                        $"{src.Doctor.Person.FirstName} {src.Doctor.Person.LastName}".Trim()))
                .ForMember(dest => dest.BranchName,
                    opt => opt.MapFrom(src => src.Branch.Name));

            // Request → Entity
            CreateMap<CreateAppointmentRequest, Appointment>()
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Branch, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Visit, opt => opt.Ignore());

            CreateMap<UpdateAppointmentRequest, Appointment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}