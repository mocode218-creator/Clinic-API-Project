using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Specializations;
using Clinic_BusinessLayer.DTOs.Responses.Specializations;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class SpecializationMappingProfile : Profile
    {
        public SpecializationMappingProfile()
        {
            // Entity → Response
            CreateMap<Specialization, SpecializationResponse>();

            // Request → Entity
            CreateMap<CreateSpecializationRequest, Specialization>();

            // Update: بس الحقول اللي مش null
            CreateMap<UpdateSpecializationRequest, Specialization>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
