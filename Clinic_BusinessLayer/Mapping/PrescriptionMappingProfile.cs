using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.PrescriptionItems;
using Clinic_BusinessLayer.DTOs.Requests.Prescriptions;
using Clinic_BusinessLayer.DTOs.Responses.Prescriptions;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class PrescriptionMappingProfile : Profile
    {
        public PrescriptionMappingProfile()
        {
            // Entity → Response
            CreateMap<Prescription, PrescriptionResponse>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.PrescriptionItems));

            CreateMap<PrescriptionItem, PrescriptionItemResponse>();

            // Request → Entity
            CreateMap<CreatePrescriptionRequest, Prescription>()
                .ForMember(dest => dest.Visit, opt => opt.Ignore())
                .ForMember(dest => dest.PrescriptionItems, opt => opt.Ignore());

            CreateMap<CreatePrescriptionItemRequest, PrescriptionItem>()
                .ForMember(dest => dest.Prescription, opt => opt.Ignore());

            CreateMap<UpdatePrescriptionRequest, Prescription>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}