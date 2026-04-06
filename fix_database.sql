-- ======================================================================
-- SCRIPT PARA ARREGLAR BASE DE DATOS SIGEBI
-- Ejecutar en SQL Server Management Studio (SSMS)
-- ======================================================================

USE SIGEBI;
GO

-- =========================================================================
-- 1. AGREGAR COLUMNA PasswordHash A ESTUDIANTES (si no existe)
-- =========================================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Object_ID = Object_ID('Estudiantes') AND name = 'PasswordHash')
BEGIN
    ALTER TABLE Estudiantes ADD PasswordHash NVARCHAR(MAX) NULL;
    PRINT 'Columna PasswordHash agregada a Estudiantes';
END
ELSE
BEGIN
    PRINT 'Columna PasswordHash ya existe en Estudiantes';
END
GO

-- =========================================================================
-- 2. AGREGAR COLUMNA PasswordHash A DOCENTES (si no existe)
-- =========================================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Object_ID = Object_ID('Docentes') AND name = 'PasswordHash')
BEGIN
    ALTER TABLE Docentes ADD PasswordHash NVARCHAR(MAX) NULL;
    PRINT 'Columna PasswordHash agregada a Docentes';
END
ELSE
BEGIN
    PRINT 'Columna PasswordHash ya existe en Docentes';
END
GO

-- =========================================================================
-- 3. AGREGAR COLUMNA Rol A ESTUDIANTES (si no existe)
-- =========================================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Object_ID = Object_ID('Estudiantes') AND name = 'Rol')
BEGIN
    ALTER TABLE Estudiantes ADD Rol INT NOT NULL DEFAULT 1;
    PRINT 'Columna Rol agregada a Estudiantes';
END
ELSE
BEGIN
    PRINT 'Columna Rol ya existe en Estudiantes';
END
GO

-- =========================================================================
-- 4. AGREGAR COLUMNA Rol A DOCENTES (si no existe)
-- =========================================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Object_ID = Object_ID('Docentes') AND name = 'Rol')
BEGIN
    ALTER TABLE Docentes ADD Rol INT NOT NULL DEFAULT 2;
    PRINT 'Columna Rol agregada a Docentes';
END
ELSE
BEGIN
    PRINT 'Columna Rol ya existe en Docentes';
END
GO

PRINT '';
PRINT '========================================';
PRINT 'SCRIPT COMPLETADO';
PRINT '========================================';
PRINT '';
PRINT 'Ahora puedes insertar datos de prueba.';
GO
