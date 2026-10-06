/* V007 - Indexes
   Unique indexes on soft-deletable tables are FILTERED (DeletedAt IS NULL),
   so a soft-deleted row does not block re-using its username / email / license number. */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE ClinicDB;
GO

/* ---------- Unique (filtered) ---------- */
CREATE UNIQUE INDEX UX_Roles_Name            ON dbo.Roles(Name)             WHERE DeletedAt IS NULL;
CREATE UNIQUE INDEX UX_Specializations_Name  ON dbo.Specializations(Name)   WHERE DeletedAt IS NULL;
CREATE UNIQUE INDEX UX_Users_UserName        ON dbo.Users(UserName)         WHERE DeletedAt IS NULL;
CREATE UNIQUE INDEX UX_Users_PersonId        ON dbo.Users(PersonId)         WHERE DeletedAt IS NULL;
CREATE UNIQUE INDEX UX_Patients_PersonId     ON dbo.Patients(PersonId)      WHERE DeletedAt IS NULL;
CREATE UNIQUE INDEX UX_Doctors_PersonId      ON dbo.Doctors(PersonId)       WHERE DeletedAt IS NULL;
CREATE UNIQUE INDEX UX_Doctors_LicenseNumber ON dbo.Doctors(LicenseNumber)  WHERE DeletedAt IS NULL;
CREATE UNIQUE INDEX UX_Persons_Email         ON dbo.Persons(Email)          WHERE DeletedAt IS NULL AND Email IS NOT NULL;
GO

/* One visit per appointment */
CREATE UNIQUE INDEX UX_Visits_AppointmentId  ON dbo.Visits(AppointmentId);

/* One schedule slot per doctor/day/start time */
CREATE UNIQUE INDEX UX_DoctorSchedule_Slot   ON dbo.DoctorSchedule(DoctorId, DayOfWeek, StartTime);
GO

/* ---------- Search / foreign-key indexes ---------- */
CREATE INDEX IX_Persons_Name              ON dbo.Persons(LastName, FirstName);
CREATE INDEX IX_Persons_Phone             ON dbo.Persons(Phone);

CREATE INDEX IX_Users_RoleId              ON dbo.Users(RoleId);
CREATE INDEX IX_Doctors_SpecializationId  ON dbo.Doctors(SpecializationId);

CREATE INDEX IX_Appointments_Patient      ON dbo.Appointments(PatientId, StartDate);
CREATE INDEX IX_Appointments_Doctor       ON dbo.Appointments(DoctorId, StartDate);
CREATE INDEX IX_Appointments_Branch       ON dbo.Appointments(BranchId, StartDate);
CREATE INDEX IX_Appointments_CreatedBy    ON dbo.Appointments(CreatedBy);
CREATE INDEX IX_Appointments_Status       ON dbo.Appointments(Status);

CREATE INDEX IX_Prescriptions_VisitId     ON dbo.Prescriptions(VisitId);
CREATE INDEX IX_PrescriptionItems_PrescId ON dbo.PrescriptionItems(PrescriptionId);

CREATE INDEX IX_Invoices_VisitId          ON dbo.Invoices(VisitId);
CREATE INDEX IX_Invoices_Status           ON dbo.Invoices(Status);
CREATE INDEX IX_Payments_InvoiceId        ON dbo.Payments(InvoiceId);

CREATE INDEX IX_AuditLog_Record           ON dbo.AuditLog(TableName, RecordId);
CREATE INDEX IX_AuditLog_UserId           ON dbo.AuditLog(UserId);
CREATE INDEX IX_AuditLog_ChangedAt        ON dbo.AuditLog(ChangedAt);
GO
