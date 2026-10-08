using System;
using System.Collections.Generic;
using Clinic_DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic_DataAccess.Context;

public partial class ClinicDbContext : DbContext
{
    public ClinicDbContext()
    {
    }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<DoctorSchedule> DoctorSchedules { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionItem> PrescriptionItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // السلسلة بتيجي من Program.cs عن طريق AddDbContext
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Appointments_SetUpdatedAt"));

            entity.HasIndex(e => new { e.BranchId, e.StartDate }, "IX_Appointments_Branch");

            entity.HasIndex(e => e.CreatedBy, "IX_Appointments_CreatedBy");

            entity.HasIndex(e => new { e.DoctorId, e.StartDate }, "IX_Appointments_Doctor");

            entity.HasIndex(e => new { e.PatientId, e.StartDate }, "IX_Appointments_Patient");

            entity.HasIndex(e => e.Status, "IX_Appointments_Status");

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Appointments_CreatedAt");
            entity.Property(e => e.EndDate).HasPrecision(0);
            entity.Property(e => e.Note)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasPrecision(0);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Scheduled", "DF_Appointments_Status");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Consultation", "DF_Appointments_Type");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Appointments_UpdatedAt");

            entity.HasOne(d => d.Branch).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Branches");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Users");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Doctors");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Patients");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLog");

            entity.HasIndex(e => e.ChangedAt, "IX_AuditLog_ChangedAt");

            entity.HasIndex(e => new { e.TableName, e.RecordId }, "IX_AuditLog_Record");

            entity.HasIndex(e => e.UserId, "IX_AuditLog_UserId");

            entity.Property(e => e.ChangedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_AuditLog_ChangedAt");
            entity.Property(e => e.Field)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NewValue).IsUnicode(false);
            entity.Property(e => e.OldValue).IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AuditLog_Users");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TR_Branches_SetUpdatedAt");
                    tb.HasTrigger("TR_Branches_SoftDelete");
                });

            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Branches_CreatedAt");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Branches_UpdatedAt");
            entity.Property(e => e.WorkingHours)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TR_Doctors_SetUpdatedAt");
                    tb.HasTrigger("TR_Doctors_SoftDelete");
                });

            entity.HasIndex(e => e.SpecializationId, "IX_Doctors_SpecializationId");

            entity.HasIndex(e => e.LicenseNumber, "UX_Doctors_LicenseNumber")
                .IsUnique()
                .HasFilter("([DeletedAt] IS NULL)");

            entity.HasIndex(e => e.PersonId, "UX_Doctors_PersonId")
                .IsUnique()
                .HasFilter("([DeletedAt] IS NULL)");

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Doctors_CreatedAt");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Doctors_UpdatedAt");

            entity.HasOne(d => d.Person).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctors_Persons");

            entity.HasOne(d => d.Specialization).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctors_Specializations");
        });

        modelBuilder.Entity<DoctorSchedule>(entity =>
        {
            entity.ToTable("DoctorSchedule", tb => tb.HasTrigger("TR_DoctorSchedule_SetUpdatedAt"));

            entity.HasIndex(e => new { e.DoctorId, e.DayOfWeek, e.StartTime }, "UX_DoctorSchedule_Slot").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_DoctorSchedule_CreatedAt");
            entity.Property(e => e.EndTime).HasPrecision(0);
            entity.Property(e => e.StartTime).HasPrecision(0);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_DoctorSchedule_UpdatedAt");

            entity.HasOne(d => d.Doctor).WithMany(p => p.DoctorSchedules)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorSchedule_Doctors");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Invoices_SetUpdatedAt"));

            entity.HasIndex(e => e.Status, "IX_Invoices_Status");

            entity.HasIndex(e => e.VisitId, "IX_Invoices_VisitId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Invoices_CreatedAt");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Unpaid", "DF_Invoices_Status");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Invoices_UpdatedAt");

            entity.HasOne(d => d.Visit).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoices_Visits");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TR_Patients_SetUpdatedAt");
                    tb.HasTrigger("TR_Patients_SoftDelete");
                });

            entity.HasIndex(e => e.PersonId, "UX_Patients_PersonId")
                .IsUnique()
                .HasFilter("([DeletedAt] IS NULL)");

            entity.Property(e => e.Allergies)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BloodType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ChronicConditions)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Patients_CreatedAt");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.EmergencyContactName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Patients_UpdatedAt");

            entity.HasOne(d => d.Person).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patients_Persons");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Payments_SetUpdatedAt"));

            entity.HasIndex(e => e.InvoiceId, "IX_Payments_InvoiceId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Payments_CreatedAt");
            entity.Property(e => e.Method)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Payments_UpdatedAt");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Payments)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Invoices");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TR_Persons_SetUpdatedAt");
                    tb.HasTrigger("TR_Persons_SoftDelete");
                });

            entity.HasIndex(e => new { e.LastName, e.FirstName }, "IX_Persons_Name");

            entity.HasIndex(e => e.Phone, "IX_Persons_Phone");

            entity.HasIndex(e => e.Email, "UX_Persons_Email")
                .IsUnique()
                .HasFilter("([DeletedAt] IS NULL AND [Email] IS NOT NULL)");

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Persons_CreatedAt");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Persons_UpdatedAt");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Prescriptions_SetUpdatedAt"));

            entity.HasIndex(e => e.VisitId, "IX_Prescriptions_VisitId");

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Prescriptions_CreatedAt");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Prescriptions_UpdatedAt");

            entity.HasOne(d => d.Visit).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prescriptions_Visits");
        });

        modelBuilder.Entity<PrescriptionItem>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_PrescriptionItems_SetUpdatedAt"));

            entity.HasIndex(e => e.PrescriptionId, "IX_PrescriptionItems_PrescId");

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_PrescriptionItems_CreatedAt");
            entity.Property(e => e.Dosage)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Duration)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Frequency)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Instructions)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MedicationName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_PrescriptionItems_UpdatedAt");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionItems)
                .HasForeignKey(d => d.PrescriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrescriptionItems_Prescriptions");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TR_Roles_SetUpdatedAt");
                    tb.HasTrigger("TR_Roles_SoftDelete");
                });

            entity.HasIndex(e => e.Name, "UX_Roles_Name")
                .IsUnique()
                .HasFilter("([DeletedAt] IS NULL)");

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Roles_CreatedAt");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Roles_UpdatedAt");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TR_Specializations_SetUpdatedAt");
                    tb.HasTrigger("TR_Specializations_SoftDelete");
                });

            entity.HasIndex(e => e.Name, "UX_Specializations_Name")
                .IsUnique()
                .HasFilter("([DeletedAt] IS NULL)");

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Specializations_CreatedAt");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Specializations_UpdatedAt");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TR_Users_SetUpdatedAt");
                    tb.HasTrigger("TR_Users_SoftDelete");
                });

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.HasIndex(e => e.PersonId, "UX_Users_PersonId")
                .IsUnique()
                .HasFilter("([DeletedAt] IS NULL)");

            entity.HasIndex(e => e.UserName, "UX_Users_UserName")
                .IsUnique()
                .HasFilter("([DeletedAt] IS NULL)");

            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Users_CreatedAt");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Users_IsActive");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Users_UpdatedAt");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Person).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Persons");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Visits_SetUpdatedAt"));

            entity.HasIndex(e => e.AppointmentId, "UX_Visits_AppointmentId").IsUnique();

            entity.Property(e => e.Bp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BP");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Visits_CreatedAt");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Notes)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.Symptoms)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Temperature).HasColumnType("decimal(4, 1)");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Visits_UpdatedAt");
            entity.Property(e => e.Weight).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Appointment).WithOne(p => p.Visit)
                .HasForeignKey<Visit>(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visits_Appointments");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
