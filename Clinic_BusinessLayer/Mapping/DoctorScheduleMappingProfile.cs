using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.DoctorSchedules;
using Clinic_BusinessLayer.DTOs.Responses.DoctorSchedules;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class DoctorScheduleMappingProfile : Profile
    {
        public DoctorScheduleMappingProfile()
        {
            CreateMap<DoctorSchedule, DoctorScheduleResponse>()
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src =>
                        $"{src.Doctor.Person.FirstName} {src.Doctor.Person.LastName}".Trim()))
                .ForMember(dest => dest.DayName,
                    opt => opt.MapFrom(src => GetDayName(src.DayOfWeek)));

            CreateMap<CreateDoctorScheduleRequest, DoctorSchedule>()
                .ForMember(dest => dest.Doctor, opt => opt.Ignore());

            CreateMap<UpdateDoctorScheduleRequest, DoctorSchedule>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

        private static string GetDayName(byte dayOfWeek)
        {
            return dayOfWeek switch
            {
                0 => "Sunday",
                1 => "Monday",
                2 => "Tuesday",
                3 => "Wednesday",
                4 => "Thursday",
                5 => "Friday",
                6 => "Saturday",
                _ => "Unknown"
            };
        }
    }
}