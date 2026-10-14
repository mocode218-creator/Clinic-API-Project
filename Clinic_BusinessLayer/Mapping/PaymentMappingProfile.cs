using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Payments;
using Clinic_BusinessLayer.DTOs.Responses.Payments;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Payment, PaymentResponse>();

            CreateMap<CreatePaymentRequest, Payment>()
                .ForMember(dest => dest.Invoice, opt => opt.Ignore());
        }
    }
}