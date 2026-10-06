/* V008 - Triggers
   1) AFTER UPDATE  : sets UpdatedAt automatically on every table that has it.
   2) INSTEAD OF DELETE : soft delete - a DELETE statement sets DeletedAt instead of removing the row
      (only for tables that have a DeletedAt column).
   CreatedAt is handled by the column DEFAULT, so no trigger is needed for it.
   Note: queries must filter "DeletedAt IS NULL" (or use an EF Core global query filter). */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE ClinicDB;
GO

DECLARE @Tables TABLE (TableName SYSNAME PRIMARY KEY, HasDeletedAt BIT NOT NULL);

INSERT INTO @Tables (TableName, HasDeletedAt) VALUES
    ('Roles',             1),
    ('Specializations',   1),
    ('Branches',          1),
    ('Persons',           1),
    ('Patients',          1),
    ('Doctors',           1),
    ('Users',             1),
    ('DoctorSchedule',    0),
    ('Appointments',      0),
    ('Visits',            0),
    ('Prescriptions',     0),
    ('PrescriptionItems', 0),
    ('Invoices',          0),
    ('Payments',          0);

DECLARE @Name SYSNAME, @HasDeleted BIT, @Sql NVARCHAR(MAX);

DECLARE tbl_cursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT TableName, HasDeletedAt FROM @Tables;

OPEN tbl_cursor;
FETCH NEXT FROM tbl_cursor INTO @Name, @HasDeleted;

WHILE @@FETCH_STATUS = 0
BEGIN
    /* ---- UpdatedAt trigger ---- */
    SET @Sql = N'
CREATE OR ALTER TRIGGER dbo.TR_' + @Name + N'_SetUpdatedAt
ON dbo.' + QUOTENAME(@Name) + N'
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE t
       SET UpdatedAt = SYSUTCDATETIME()
      FROM dbo.' + QUOTENAME(@Name) + N' AS t
      JOIN inserted AS i ON i.Id = t.Id;
END';
    EXEC (@Sql);

    /* ---- Soft delete trigger ---- */
    IF @HasDeleted = 1
    BEGIN
        SET @Sql = N'
CREATE OR ALTER TRIGGER dbo.TR_' + @Name + N'_SoftDelete
ON dbo.' + QUOTENAME(@Name) + N'
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE t
       SET DeletedAt = SYSUTCDATETIME(),
           UpdatedAt = SYSUTCDATETIME()
      FROM dbo.' + QUOTENAME(@Name) + N' AS t
      JOIN deleted AS d ON d.Id = t.Id
     WHERE t.DeletedAt IS NULL;
END';
        EXEC (@Sql);
    END

    FETCH NEXT FROM tbl_cursor INTO @Name, @HasDeleted;
END

CLOSE tbl_cursor;
DEALLOCATE tbl_cursor;
GO
