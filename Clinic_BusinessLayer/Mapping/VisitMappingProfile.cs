using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Visits;
using Clinic_BusinessLayer.DTOs.Responses.Visits;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class VisitMappingProfile : Profile
    {
        public VisitMappingProfile()
        {
            CreateMap<Visit, VisitResponse>();

            CreateMap<CreateVisitRequest, Visit>()
                .ForMember(dest => dest.Appointment, opt => opt.Ignore())
                .ForMember(dest => dest.Invoices, opt => opt.Ignore())
                .ForMember(dest => dest.Prescriptions, opt => opt.Ignore());

            CreateMap<UpdateVisitRequest, Visit>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}