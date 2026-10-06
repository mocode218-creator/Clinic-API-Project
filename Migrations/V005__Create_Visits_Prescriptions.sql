/* V005 - Visits, Prescriptions, PrescriptionItems */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE ClinicDB;
GO

IF OBJECT_ID(N'dbo.Visits', N'U') IS NULL
CREATE TABLE dbo.Visits (
    Id             INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Visits PRIMARY KEY,
    AppointmentId  INT               NOT NULL CONSTRAINT FK_Visits_Appointments REFERENCES dbo.Appointments(Id),
    Symptoms       VARCHAR(1000)     NULL,
    Diagnosis      VARCHAR(1000)     NULL,
    Notes          VARCHAR(2000)     NULL,
    BP             VARCHAR(10)       NULL,          -- e.g. 120/80
    Temperature    DECIMAL(4,1)      NULL,          -- Celsius
    Weight         DECIMAL(5,2)      NULL,          -- Kg
    CreatedAt      DATETIME2(3)      NOT NULL CONSTRAINT DF_Visits_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt      DATETIME2(3)      NOT NULL CONSTRAINT DF_Visits_UpdatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_Visits_Temperature CHECK (Temperature IS NULL OR Temperature BETWEEN 25 AND 45),
    CONSTRAINT CK_Visits_Weight      CHECK (Weight IS NULL OR Weight > 0)
);
GO

IF OBJECT_ID(N'dbo.Prescriptions', N'U') IS NULL
CREATE TABLE dbo.Prescriptions (
    Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Prescriptions PRIMARY KEY,
    VisitId    INT               NOT NULL CONSTRAINT FK_Prescriptions_Visits REFERENCES dbo.Visits(Id),
    Notes      VARCHAR(1000)     NULL,
    CreatedAt  DATETIME2(3)      NOT NULL CONSTRAINT DF_Prescriptions_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt  DATETIME2(3)      NOT NULL CONSTRAINT DF_Prescriptions_UpdatedAt DEFAULT SYSUTCDATETIME()
);
GO

IF OBJECT_ID(N'dbo.PrescriptionItems', N'U') IS NULL
CREATE TABLE dbo.PrescriptionItems (
    Id              INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PrescriptionItems PRIMARY KEY,
    PrescriptionId  INT               NOT NULL CONSTRAINT FK_PrescriptionItems_Prescriptions REFERENCES dbo.Prescriptions(Id),
    MedicationName  VARCHAR(150)      NOT NULL,
    Dosage          VARCHAR(100)      NOT NULL,
    Frequency       VARCHAR(100)      NOT NULL,
    Duration        VARCHAR(100)      NOT NULL,
    Instructions    VARCHAR(500)      NULL,
    CreatedAt       DATETIME2(3)      NOT NULL CONSTRAINT DF_PrescriptionItems_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt       DATETIME2(3)      NOT NULL CONSTRAINT DF_PrescriptionItems_UpdatedAt DEFAULT SYSUTCDATETIME()
);
GO
