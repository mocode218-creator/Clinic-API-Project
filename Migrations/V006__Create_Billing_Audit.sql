/* V006 - Invoices, Payments, AuditLog */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE ClinicDB;
GO

IF OBJECT_ID(N'dbo.Invoices', N'U') IS NULL
CREATE TABLE dbo.Invoices (
    Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Invoices PRIMARY KEY,
    VisitId    INT               NOT NULL CONSTRAINT FK_Invoices_Visits REFERENCES dbo.Visits(Id),
    Status     VARCHAR(20)       NOT NULL CONSTRAINT DF_Invoices_Status DEFAULT 'Unpaid',
    Amount     DECIMAL(18,2)     NOT NULL,
    CreatedAt  DATETIME2(3)      NOT NULL CONSTRAINT DF_Invoices_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt  DATETIME2(3)      NOT NULL CONSTRAINT DF_Invoices_UpdatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_Invoices_Status CHECK (Status IN ('Unpaid','PartiallyPaid','Paid','Cancelled','Refunded')),
    CONSTRAINT CK_Invoices_Amount CHECK (Amount >= 0)
);
GO

IF OBJECT_ID(N'dbo.Payments', N'U') IS NULL
CREATE TABLE dbo.Payments (
    Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Payments PRIMARY KEY,
    InvoiceId  INT               NOT NULL CONSTRAINT FK_Payments_Invoices REFERENCES dbo.Invoices(Id),
    Amount     DECIMAL(18,2)     NOT NULL,
    Method     VARCHAR(20)       NOT NULL,
    CreatedAt  DATETIME2(3)      NOT NULL CONSTRAINT DF_Payments_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt  DATETIME2(3)      NOT NULL CONSTRAINT DF_Payments_UpdatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_Payments_Amount CHECK (Amount > 0),
    CONSTRAINT CK_Payments_Method CHECK (Method IN ('Cash','Card','Insurance','BankTransfer','Wallet'))
);
GO

IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NULL
CREATE TABLE dbo.AuditLog (
    Id         BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLog PRIMARY KEY,
    UserId     INT                  NULL CONSTRAINT FK_AuditLog_Users REFERENCES dbo.Users(Id),
    TableName  VARCHAR(100)         NOT NULL,
    RecordId   BIGINT               NOT NULL,
    Field      VARCHAR(100)         NOT NULL,
    OldValue   VARCHAR(MAX)         NULL,
    NewValue   VARCHAR(MAX)         NULL,
    ChangedAt  DATETIME2(3)         NOT NULL CONSTRAINT DF_AuditLog_ChangedAt DEFAULT SYSUTCDATETIME()
);
GO
