/* V004 - Appointments */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE ClinicDB;
GO

IF OBJECT_ID(N'dbo.Appointments', N'U') IS NULL
CREATE TABLE dbo.Appointments (
    Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Appointments PRIMARY KEY,
    PatientId   INT               NOT NULL CONSTRAINT FK_Appointments_Patients  REFERENCES dbo.Patients(Id),
    DoctorId    INT               NOT NULL CONSTRAINT FK_Appointments_Doctors   REFERENCES dbo.Doctors(Id),
    BranchId    INT               NOT NULL CONSTRAINT FK_Appointments_Branches  REFERENCES dbo.Branches(Id),
    CreatedBy   INT               NOT NULL CONSTRAINT FK_Appointments_Users     REFERENCES dbo.Users(Id),
    StartDate   DATETIME2(0)      NOT NULL,
    EndDate     DATETIME2(0)      NOT NULL,
    Type        VARCHAR(30)       NOT NULL CONSTRAINT DF_Appointments_Type   DEFAULT 'Consultation',
    Note        VARCHAR(500)      NULL,
    Status      VARCHAR(20)       NOT NULL CONSTRAINT DF_Appointments_Status DEFAULT 'Scheduled',
    CreatedAt   DATETIME2(3)      NOT NULL CONSTRAINT DF_Appointments_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt   DATETIME2(3)      NOT NULL CONSTRAINT DF_Appointments_UpdatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_Appointments_Dates  CHECK (EndDate > StartDate),
    CONSTRAINT CK_Appointments_Type   CHECK (Type   IN ('Consultation','FollowUp','Emergency','Procedure')),
    CONSTRAINT CK_Appointments_Status CHECK (Status IN ('Scheduled','Confirmed','CheckedIn','Completed','Cancelled','NoShow'))
);
GO
