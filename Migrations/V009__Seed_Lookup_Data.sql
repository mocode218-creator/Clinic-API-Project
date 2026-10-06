/* V009 - Seed lookup data (idempotent: safe to run more than once) */
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE ClinicDB;
GO

/* ---------- Roles ---------- */
INSERT INTO dbo.Roles (Name)
SELECT v.Name
FROM (VALUES ('Admin'), ('Doctor'), ('Receptionist')) AS v(Name)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Roles r WHERE r.Name = v.Name AND r.DeletedAt IS NULL);
GO

/* ---------- Specializations ---------- */
INSERT INTO dbo.Specializations (Name)
SELECT v.Name
FROM (VALUES
    ('General Medicine'),
    ('Pediatrics'),
    ('Internal Medicine'),
    ('Cardiology'),
    ('Dermatology'),
    ('Orthopedics'),
    ('Neurology'),
    ('Ophthalmology'),
    ('ENT'),
    ('Obstetrics and Gynecology'),
    ('Dentistry'),
    ('Psychiatry'),
    ('Urology')
) AS v(Name)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Specializations s WHERE s.Name = v.Name AND s.DeletedAt IS NULL);
GO
