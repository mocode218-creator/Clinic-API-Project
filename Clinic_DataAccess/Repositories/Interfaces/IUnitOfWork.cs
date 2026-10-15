using Clinic_DataAccess.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ISpecializationRepository Specializations { get; }
    IRoleRepository Roles { get; }
    IBranchRepository Branches { get; }
    IPersonRepository Persons { get; }
    IPatientRepository Patients { get; }
    IDoctorRepository Doctors { get; }
    IUserRepository Users { get; }
    IDoctorScheduleRepository DoctorSchedules { get; }
    IAppointmentRepository Appointments { get; }
    IVisitRepository Visits { get; }
    IPrescriptionRepository Prescriptions { get; }
    IPrescriptionItemRepository PrescriptionItems { get; }
    IInvoiceRepository Invoices { get; }
    IPaymentRepository Payments { get; }
    IAuditLogRepository AuditLogs { get; }
    IRefreshTokenRepository RefreshTokens { get; }   // ← 🆕

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}