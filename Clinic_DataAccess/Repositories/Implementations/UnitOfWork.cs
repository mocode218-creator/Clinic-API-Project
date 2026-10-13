using Clinic_DataAccess.Context;
using Clinic_DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;



namespace Clinic_DataAccess.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicDbContext _context;
        private IDbContextTransaction? _transaction;

        private ISpecializationRepository? _specializations;
        private IRoleRepository? _roles;
        private IBranchRepository? _branches;
        private IPersonRepository? _persons;
        private IPatientRepository? _patients;
        private IDoctorRepository? _doctors;
        private IUserRepository? _users;
        private IDoctorScheduleRepository? _doctorSchedules;
        private IAppointmentRepository? _appointments;
        private IVisitRepository? _visits;
        private IPrescriptionRepository? _prescriptions;
        private IPrescriptionItemRepository? _prescriptionItems;
        private IInvoiceRepository? _invoices;
        private IPaymentRepository? _payments;
        private IAuditLogRepository? _auditLogs;

        public UnitOfWork(ClinicDbContext context)
        {
            _context = context;
        }

        public ISpecializationRepository Specializations =>
            _specializations ??= new SpecializationRepository(_context);

        public IRoleRepository Roles =>
            _roles ??= new RoleRepository(_context);

        public IBranchRepository Branches =>
            _branches ??= new BranchRepository(_context);

        public IPersonRepository Persons =>
            _persons ??= new PersonRepository(_context);

        public IPatientRepository Patients =>
            _patients ??= new PatientRepository(_context);

        public IDoctorRepository Doctors =>
            _doctors ??= new DoctorRepository(_context);

        public IUserRepository Users =>
            _users ??= new UserRepository(_context);

        public IDoctorScheduleRepository DoctorSchedules =>
            _doctorSchedules ??= new DoctorScheduleRepository(_context);

        public IAppointmentRepository Appointments =>
            _appointments ??= new AppointmentRepository(_context);

        public IVisitRepository Visits =>
            _visits ??= new VisitRepository(_context);

        public IPrescriptionRepository Prescriptions =>
            _prescriptions ??= new PrescriptionRepository(_context);

        public IPrescriptionItemRepository PrescriptionItems =>
            _prescriptionItems ??= new PrescriptionItemRepository(_context);

        public IInvoiceRepository Invoices =>
            _invoices ??= new InvoiceRepository(_context);

        public IPaymentRepository Payments =>
            _payments ??= new PaymentRepository(_context);

        public IAuditLogRepository AuditLogs =>
            _auditLogs ??= new AuditLogRepository(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
            => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
