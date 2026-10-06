/* V003 - Persons, Patients, Doctors, Users, DoctorSchedule */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE ClinicDB;
GO

IF OBJECT_ID(N'dbo.Persons', N'U') IS NULL
CREATE TABLE dbo.Persons (
    Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Persons PRIMARY KEY,
    FirstName   VARCHAR(50)       NOT NULL,
    MiddleName  VARCHAR(50)       NULL,
    LastName    VARCHAR(50)       NOT NULL,
    BirthDate   DATE              NULL,
    Phone       VARCHAR(20)       NULL,
    Email       VARCHAR(150)      NULL,
    Gender      VARCHAR(10)       NULL,
    CreatedAt   DATETIME2(3)      NOT NULL CONSTRAINT DF_Persons_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt   DATETIME2(3)      NOT NULL CONSTRAINT DF_Persons_UpdatedAt DEFAULT SYSUTCDATETIME(),
    DeletedAt   DATETIME2(3)      NULL,
    CONSTRAINT CK_Persons_Gender    CHECK (Gender IN ('Male', 'Female')),
    CONSTRAINT CK_Persons_BirthDate CHECK (BirthDate <= CAST(SYSUTCDATETIME() AS DATE) OR BirthDate IS NULL)
);
GO

IF OBJECT_ID(N'dbo.Patients', N'U') IS NULL
CREATE TABLE dbo.Patients (
    Id                      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Patients PRIMARY KEY,
    PersonId                INT               NOT NULL CONSTRAINT FK_Patients_Persons REFERENCES dbo.Persons(Id),
    EmergencyContactName    VARCHAR(100)      NULL,
    EmergencyContactPhone   VARCHAR(20)       NULL,
    BloodType               VARCHAR(3)        NULL,
    Allergies               VARCHAR(500)      NULL,
    ChronicConditions       VARCHAR(500)      NULL,
    CreatedAt               DATETIME2(3)      NOT NULL CONSTRAINT DF_Patients_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt               DATETIME2(3)      NOT NULL CONSTRAINT DF_Patients_UpdatedAt DEFAULT SYSUTCDATETIME(),
    DeletedAt               DATETIME2(3)      NULL,
    CONSTRAINT CK_Patients_BloodType CHECK (BloodType IN ('A+','A-','B+','B-','AB+','AB-','O+','O-'))
);
GO

IF OBJECT_ID(N'dbo.Doctors', N'U') IS NULL
CREATE TABLE dbo.Doctors (
    Id                INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Doctors PRIMARY KEY,
    PersonId          INT               NOT NULL CONSTRAINT FK_Doctors_Persons         REFERENCES dbo.Persons(Id),
    SpecializationId  INT               NOT NULL CONSTRAINT FK_Doctors_Specializations REFERENCES dbo.Specializations(Id),
    LicenseNumber     VARCHAR(50)       NOT NULL,
    CreatedAt         DATETIME2(3)      NOT NULL CONSTRAINT DF_Doctors_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt         DATETIME2(3)      NOT NULL CONSTRAINT DF_Doctors_UpdatedAt DEFAULT SYSUTCDATETIME(),
    DeletedAt         DATETIME2(3)      NULL
);
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
CREATE TABLE dbo.Users (
    Id            INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    RoleId        INT               NOT NULL CONSTRAINT FK_Users_Roles   REFERENCES dbo.Roles(Id),
    PersonId      INT               NOT NULL CONSTRAINT FK_Users_Persons REFERENCES dbo.Persons(Id),
    UserName      VARCHAR(50)       NOT NULL,
    PasswordHash  VARCHAR(255)      NOT NULL,   -- store a hash (e.g. BCrypt/PBKDF2), never the plain password
    IsActive      BIT               NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1,
    CreatedAt     DATETIME2(3)      NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt     DATETIME2(3)      NOT NULL CONSTRAINT DF_Users_UpdatedAt DEFAULT SYSUTCDATETIME(),
    DeletedAt     DATETIME2(3)      NULL
);
GO

IF OBJECT_ID(N'dbo.DoctorSchedule', N'U') IS NULL
CREATE TABLE dbo.DoctorSchedule (
    Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_DoctorSchedule PRIMARY KEY,
    DoctorId   INT               NOT NULL CONSTRAINT FK_DoctorSchedule_Doctors REFERENCES dbo.Doctors(Id),
    DayOfWeek  TINYINT           NOT NULL,   -- 0 = Sunday ... 6 = Saturday
    StartTime  TIME(0)           NOT NULL,
    EndTime    TIME(0)           NOT NULL,
    CreatedAt  DATETIME2(3)      NOT NULL CONSTRAINT DF_DoctorSchedule_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt  DATETIME2(3)      NOT NULL CONSTRAINT DF_DoctorSchedule_UpdatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_DoctorSchedule_Day  CHECK (DayOfWeek BETWEEN 0 AND 6),
    CONSTRAINT CK_DoctorSchedule_Time CHECK (EndTime > StartTime)
);
GO
