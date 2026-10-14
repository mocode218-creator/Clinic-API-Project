using AutoMapper;
using Clinic_BusinessLayer.DTOs.Responses.Invoices;
using Clinic_DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Mapping
{
    public class InvoiceMappingProfile : Profile
    {
        public InvoiceMappingProfile()
        {
            CreateMap<Invoice, InvoiceResponse>();
            // مفيش Create/Update Request — الفاتورة بتتعمل تلقائي
        }
    }
}