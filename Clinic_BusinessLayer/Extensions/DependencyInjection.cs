using Clinic_BusinessLayer.Helpers;
using Clinic_BusinessLayer.Services.Implementations;
using Clinic_BusinessLayer.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Clinic_BusinessLayer.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // ✅ AutoMapper 13+
            services.AddAutoMapper(cfg => { }, assembly);

            // FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            // JwtHelper
            services.AddScoped<JwtHelper>();

            // Services
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IVisitService, VisitService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAuditLogService, AuditLogService>();

            return services;
        }
    }
}