/* V010 - RefreshTokens (JWT refresh-token rotation)
   Tokens are never updated except to be revoked, and never deleted,
   so this table has no UpdatedAt / DeletedAt (same idea as Payments / Visits). */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE ClinicDB;
GO

IF OBJECT_ID(N'dbo.RefreshTokens', N'U') IS NULL
CREATE TABLE dbo.RefreshTokens (
    Id               INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_RefreshTokens PRIMARY KEY,
    UserId           INT               NOT NULL CONSTRAINT FK_RefreshTokens_Users REFERENCES dbo.Users(Id),
    Token            VARCHAR(500)      NOT NULL,
    ExpiresAt        DATETIME2(3)      NOT NULL,
    CreatedAt        DATETIME2(3)      NOT NULL CONSTRAINT DF_RefreshTokens_CreatedAt DEFAULT SYSUTCDATETIME(),
    RevokedAt        DATETIME2(3)      NULL,
    ReplacedByToken  VARCHAR(500)      NULL
);
GO

/* Token lookup on every refresh request; must be unique */
CREATE UNIQUE INDEX UX_RefreshTokens_Token ON dbo.RefreshTokens(Token);

/* Find / revoke all active tokens of a user (logout, password change) */
CREATE INDEX IX_RefreshTokens_UserId ON dbo.RefreshTokens(UserId) INCLUDE (ExpiresAt, RevokedAt);
GO
