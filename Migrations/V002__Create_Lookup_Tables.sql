/* V002 - Lookup tables: Roles, Specializations, Branches
   All timestamps are UTC. CreatedAt/UpdatedAt/DeletedAt are maintained automatically
   (defaults here, triggers in V008). */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE ClinicDB;
GO

IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
CREATE TABLE dbo.Roles (
    Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Roles PRIMARY KEY,
    Name        VARCHAR(50)       NOT NULL,
    CreatedAt   DATETIME2(3)      NOT NULL CONSTRAINT DF_Roles_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt   DATETIME2(3)      NOT NULL CONSTRAINT DF_Roles_UpdatedAt DEFAULT SYSUTCDATETIME(),
    DeletedAt   DATETIME2(3)      NULL
);
GO

IF OBJECT_ID(N'dbo.Specializations', N'U') IS NULL
CREATE TABLE dbo.Specializations (
    Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Specializations PRIMARY KEY,
    Name        VARCHAR(100)      NOT NULL,
    CreatedAt   DATETIME2(3)      NOT NULL CONSTRAINT DF_Specializations_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt   DATETIME2(3)      NOT NULL CONSTRAINT DF_Specializations_UpdatedAt DEFAULT SYSUTCDATETIME(),
    DeletedAt   DATETIME2(3)      NULL
);
GO

IF OBJECT_ID(N'dbo.Branches', N'U') IS NULL
CREATE TABLE dbo.Branches (
    Id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Branches PRIMARY KEY,
    Name         VARCHAR(100)      NOT NULL,
    Address      VARCHAR(250)      NOT NULL,
    Phone        VARCHAR(20)       NULL,
    WorkingHours VARCHAR(100)      NULL,
    CreatedAt    DATETIME2(3)      NOT NULL CONSTRAINT DF_Branches_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt    DATETIME2(3)      NOT NULL CONSTRAINT DF_Branches_UpdatedAt DEFAULT SYSUTCDATETIME(),
    DeletedAt    DATETIME2(3)      NULL
);
GO
