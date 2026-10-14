using AutoMapper;
using Clinic_BusinessLayer.DTOs.Responses.AuditLogs;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class AuditLogMappingProfile : Profile
    {
        public AuditLogMappingProfile()
        {
            CreateMap<AuditLog, AuditLogResponse>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User != null
                        ? $"{src.User.Person.FirstName} {src.User.Person.LastName}".Trim()
                        : null));
        }
    }
}
