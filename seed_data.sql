-- ======================================================================
-- SCRIPT PARA POBLAR BASE DE DATOS SIGEBI CON DATOS DE PRUEBA
-- Ejecutar DESPUÉS de fix_database.sql
-- ======================================================================

USE SIGEBI;
GO

-- =========================================================================
-- 1. INSERTAR CATEGORIAS
-- =========================================================================
IF NOT EXISTS (SELECT * FROM Categorias WHERE Id = 1)
BEGIN
    INSERT INTO Categorias (Nombre) VALUES 
    ('Literatura'),
    ('Ciencia'),
    ('Historia'),
    ('Tecnología'),
    ('Matemáticas'),
    ('Arte'),
    ('Filosofía'),
    ('Deportes');
    
    PRINT 'Categorias insertadas';
END
ELSE
BEGIN
    PRINT 'Categorias ya existen';
END
GO

-- =========================================================================
-- 2. INSERTAR ESTUDIANTES
-- =========================================================================
IF NOT EXISTS (SELECT * FROM Estudiantes WHERE Id = 1)
BEGIN
    INSERT INTO Estudiantes (Nombre, Matricula, Email, Telefono, Carrera, Estado, PasswordHash, Rol) VALUES 
    ('María García López', '2024-0001', 'maria.garcia@universidad.edu', '809-555-0001', 'Ingeniería de Sistemas', 1, NULL, 1),
    ('Juan Pérez Rodríguez', '2024-0002', 'juan.perez@universidad.edu', '809-555-0002', 'Medicina', 1, NULL, 1),
    ('Ana López Martínez', '2024-0003', 'ana.lopez@universidad.edu', '809-555-0003', 'Derecho', 1, NULL, 1),
    ('Carlos Ruiz Sánchez', '2024-0004', 'carlos.ruiz@universidad.edu', '809-555-0004', 'Administración de Empresas', 1, NULL, 1),
    ('Laura Martínez Cruz', '2024-0005', 'laura.martinez@universidad.edu', '809-555-0005', 'Psicología', 1, NULL, 1),
    ('Pedro Sánchez Torres', '2024-0006', 'pedro.sanchez@universidad.edu', '809-555-0006', 'Economía', 1, NULL, 1),
    ('Sofía Torres Flores', '2024-0007', 'sofia.torres@universidad.edu', '809-555-0007', 'Comunicación Social', 1, NULL, 1),
    ('Diego Ramírez Mora', '2024-0008', 'diego.ramirez@universidad.edu', '809-555-0008', 'Arquitectura', 1, NULL, 1),
    ('Carmen Flores Díaz', '2024-0009', 'carmen.flores@universidad.edu', '809-555-0009', 'Veterinaria', 1, NULL, 1),
    ('Andrés Castro Vega', '2024-0010', 'andres.castro@universidad.edu', '809-555-0010', 'Agronomía', 1, NULL, 1);
    
    PRINT 'Estudiantes insertados';
END
ELSE
BEGIN
    PRINT 'Estudiantes ya existen';
END
GO

-- =========================================================================
-- 3. INSERTAR DOCENTES
-- =========================================================================
IF NOT EXISTS (SELECT * FROM Docentes WHERE Id = 1)
BEGIN
    INSERT INTO Docentes (Nombre, Cedula, Email, Telefono, Departamento, Estado, PasswordHash, Rol) VALUES 
    ('Dr. Roberto Mendoza Jiménez', '001-1234567-1', 'roberto.mendoza@universidad.edu', '809-555-1001', 'Ciencias Básicas', 1, NULL, 2),
    ('Dra. Patricia Vega Luna', '002-2345678-2', 'patricia.vega@universidad.edu', '809-555-1002', 'Humanidades', 1, NULL, 2),
    ('Dr. Fernando Mora Castillo', '003-3456789-3', 'fernando.mora@universidad.edu', '809-555-1003', 'Ingeniería', 1, NULL, 2),
    ('Dra. Isabel Fuentes Ramírez', '004-4567890-4', 'isabel.fuentes@universidad.edu', '809-555-1004', 'Medicina', 1, NULL, 2),
    ('Dr. Jorge Luna Herrera', '005-5678901-5', 'jorge.luna@universidad.edu', '809-555-1005', 'Economía', 1, NULL, 2);
    
    PRINT 'Docentes insertados';
END
ELSE
BEGIN
    PRINT 'Docentes ya existen';
END
GO

-- =========================================================================
-- 4. INSERTAR LIBROS
-- =========================================================================
IF NOT EXISTS (SELECT * FROM Libros WHERE Id = 1)
BEGIN
    INSERT INTO Libros (Titulo, Autor, ISBN, Ubicacion, Editorial, Stock, StockDisponible, Estado, FechaAdquisicion, CategoriaId) VALUES 
    ('Cien Años de Soledad', 'Gabriel García Márquez', '978-0-06-088328-7', 'Estante A-1', 'Editorial Sudamericana', 3, 3, 1, GETDATE(), 1),
    ('Don Quijote de la Mancha', 'Miguel de Cervantes', '978-0-14-243723-0', 'Estante A-2', 'Planeta', 2, 2, 1, GETDATE(), 1),
    ('La Ilíada', 'Homero', '978-0-14-027536-0', 'Estante A-3', 'Penguin Classics', 2, 2, 1, GETDATE(), 1),
    ('Breves Respuestas a las Grandes Preguntas', 'Stephen Hawking', '978-0-316-55639-3', 'Estante B-1', 'Bantam', 3, 3, 1, GETDATE(), 2),
    ('El Gene: Una Historia Intima', 'Siddhartha Mukherjee', '978-1-4767-3352-6', 'Estante B-2', 'Scribner', 2, 2, 1, GETDATE(), 2),
    ('Cosmos', 'Carl Sagan', '978-0-345-53943-1', 'Estante B-3', 'Ballantine Books', 3, 3, 1, GETDATE(), 2),
    ('Sapiens: De Animales a Dioses', 'Yuval Noah Harari', '978-0-06-231609-7', 'Estante C-1', 'Harper', 4, 4, 1, GETDATE(), 3),
    ('Historia de Roma', 'Indro Montanelli', '978-0-06-008665-9', 'Estante C-2', 'Harper Perennial', 2, 2, 1, GETDATE(), 3),
    ('El Mundo de Yesterday', 'Stefan Zweig', '978-0-14-310758-1', 'Estante C-3', 'Penguin Classics', 2, 2, 1, GETDATE(), 3),
    ('Clean Code', 'Robert C. Martin', '978-0-13-235088-4', 'Estante D-1', 'Prentice Hall', 3, 3, 1, GETDATE(), 4),
    ('The Pragmatic Programmer', 'David Thomas, Andrew Hunt', '978-0-13-595705-9', 'Estante D-2', 'Addison-Wesley', 2, 2, 1, GETDATE(), 4),
    ('Introduction to Algorithms', 'Thomas H. Cormen', '978-0-262-03384-8', 'Estante D-3', 'MIT Press', 3, 3, 1, GETDATE(), 5),
    ('Cálculo Diferencial e Integral', 'Silvia Rawn', '978-0-07-353247-5', 'Estante D-4', 'McGraw-Hill', 2, 2, 1, GETDATE(), 5),
    ('El Cuaderno de Verano', 'Varios', '978-0-14-044505-0', 'Estante E-1', 'Penguin', 5, 5, 1, GETDATE(), 6),
    ('Historia del Arte', 'E.H. Gombrich', '978-0-06-443630-6', 'Estante E-2', 'Phaidon Press', 2, 2, 1, GETDATE(), 6),
    ('El Ser y el Tiempo', 'Martin Heidegger', '978-0-06-157559-4', 'Estante F-1', 'Harper Perennial', 2, 2, 1, GETDATE(), 7),
    ('Meditaciones', 'Marco Aurelio', '978-0-14-044933-4', 'Estante F-2', 'Penguin Classics', 3, 3, 1, GETDATE(), 7),
    ('El Arte de la Guerra', 'Sun Tzu', '978-1-59030-225-2', 'Estante F-3', 'Shambhala', 2, 2, 1, GETDATE(), 7),
    ('Fútbol Total', 'Joh Cruyff', '978-0-07-142040-1', 'Estante G-1', 'McGraw-Hill', 2, 2, 1, GETDATE(), 8),
    ('Mi Revolución Ética', 'Pep Guardiola', '978-0-06-249956-4', 'Estante G-2', 'Harper', 2, 2, 1, GETDATE(), 8),
    ('El Código del赶', 'Robert Greene', '978-0-14-311-171-1', 'Estante H-1', 'Viking', 3, 3, 1, GETDATE(), 4),
    ('Thinking Fast and Slow', 'Daniel Kahneman', '978-0-374-53355-7', 'Estante H-2', 'Farrar, Straus and Giroux', 2, 2, 1, GETDATE(), 7),
    ('La Psicología del Dinero', 'Morgan Housel', '978-0-06-307689-6', 'Estante H-3', 'Houghton Mifflin', 4, 4, 1, GETDATE(), 4),
    ('Guns Germs and Steel', 'Jared Diamond', '978-0-393-31755-8', 'Estante I-1', 'W. W. Norton', 2, 2, 1, GETDATE(), 3),
    ('El Hombre que Amaba a los Perros', 'Leonardo Padura', '978-0-8021-4659-1', 'Estante I-2', 'Bloomsbury', 2, 2, 1, GETDATE(), 1),
    ('La Metamorfosis', 'Franz Kafka', '978-0-14-310577-6', 'Estante I-3', 'Penguin Classics', 3, 3, 1, GETDATE(), 1),
    ('Física para la Ciencia y la Tecnología', 'Paul A. Tipler', '978-0-7167-4364-3', 'Estante J-1', 'W. H. Freeman', 2, 2, 1, GETDATE(), 2),
    ('Aprende a Dibujar', 'Betty Edwards', '978-0-399-51616-6', 'Estante J-2', 'TarcherPerigee', 3, 3, 1, GETDATE(), 6),
    ('1984', 'George Orwell', '978-0-14-028333-4', 'Estante J-3', 'Penguin Classics', 4, 4, 1, GETDATE(), 1);
    
    PRINT 'Libros insertados';
END
ELSE
BEGIN
    PRINT 'Libros ya existen';
END
GO

-- =========================================================================
-- 5. INSERTAR ALGUNOS PRÉSTAMOS DE PRUEBA
-- =========================================================================
IF NOT EXISTS (SELECT * FROM Prestamos WHERE Id = 1)
BEGIN
    -- Préstamos activos (fecha límite futura)
    INSERT INTO Prestamos (FechaPrestamo, FechaLimite, FechaDevolucionReal, Renovaciones, Estado, LibroId, EstudianteId, DocenteId) VALUES 
    (GETDATE()-3, GETDATE()+4, NULL, 0, 1, 1, 1, NULL),
    (GETDATE()-5, GETDATE()+2, NULL, 0, 1, 4, 2, NULL),
    (GETDATE()-1, GETDATE()+6, NULL, 0, 1, 10, 3, NULL),
    (GETDATE()-2, GETDATE()+5, NULL, 0, 1, 7, NULL, 1),
    (GETDATE()-4, GETDATE()+3, NULL, 0, 1, 15, NULL, 2);
    
    -- Préstamos vencidos (fecha límite pasada)
    INSERT INTO Prestamos (FechaPrestamo, FechaLimite, FechaDevolucionReal, Renovaciones, Estado, LibroId, EstudianteId, DocenteId) VALUES 
    (GETDATE()-15, GETDATE()-8, NULL, 0, 3, 2, 4, NULL),
    (GETDATE()-12, GETDATE()-5, NULL, 0, 3, 8, 5, NULL);
    
    -- Préstamos devueltos
    INSERT INTO Prestamos (FechaPrestamo, FechaLimite, FechaDevolucionReal, Renovaciones, Estado, LibroId, EstudianteId, DocenteId) VALUES 
    (GETDATE()-20, GETDATE()-13, GETDATE()-14, 0, 2, 3, 6, NULL),
    (GETDATE()-18, GETDATE()-11, GETDATE()-11, 0, 2, 5, 7, NULL),
    (GETDATE()-25, GETDATE()-18, GETDATE()-18, 1, 2, 6, NULL, 3);
    
    -- Actualizar stock de libros prestados
    UPDATE Libros SET StockDisponible = StockDisponible - 1 WHERE Id IN (1, 4, 10, 7, 15, 2, 8, 3, 5, 6);
    
    PRINT 'Préstamos de prueba insertados';
END
ELSE
BEGIN
    PRINT 'Préstamos ya existen';
END
GO

-- =========================================================================
-- 6. INSERTAR ALGUNAS RESERVAS DE PRUEBA
-- =========================================================================
IF NOT EXISTS (SELECT * FROM Reservas WHERE Id = 1)
BEGIN
    -- Reservas pendientes
    INSERT INTO Reservas (FechaReserva, FechaExpiracion, PosicionCola, Estado, LibroId, EstudianteId, DocenteId) VALUES 
    (GETDATE()-1, GETDATE()+2, 1, 1, 1, 8, NULL),
    (GETDATE()-2, GETDATE()+1, 1, 1, 4, 9, NULL),
    (GETDATE()-1, GETDATE()+2, 1, 1, 10, NULL, 4);
    
    -- Reservas completadas
    INSERT INTO Reservas (FechaReserva, FechaExpiracion, PosicionCola, Estado, LibroId, EstudianteId, DocenteId) VALUES 
    (GETDATE()-30, GETDATE()-27, 1, 3, 11, 1, NULL),
    (GETDATE()-25, GETDATE()-22, 1, 3, 12, NULL, 1);
    
    PRINT 'Reservas de prueba insertadas';
END
ELSE
BEGIN
    PRINT 'Reservas ya existen';
END
GO

-- =========================================================================
-- VERIFICACIÓN
-- =========================================================================
PRINT '';
PRINT '========================================';
PRINT 'DATOS DE PRUEBA INSERTADOS';
PRINT '========================================';
PRINT '';

SELECT 'Categorias:' AS Tipo, COUNT(*) AS Cantidad FROM Categorias
UNION ALL
SELECT 'Estudiantes:', COUNT(*) FROM Estudiantes
UNION ALL
SELECT 'Docentes:', COUNT(*) FROM Docentes
UNION ALL
SELECT 'Libros:', COUNT(*) FROM Libros
UNION ALL
SELECT 'Préstamos:', COUNT(*) FROM Prestamos
UNION ALL
SELECT 'Reservas:', COUNT(*) FROM Reservas;

PRINT '';
PRINT '========================================';
PRINT 'CREDENCIALES DE LOGIN:';
PRINT '========================================';
PRINT 'ESTUDIANTE: Matrícula 2024-0001, Password 2024-0001';
PRINT 'ESTUDIANTE: Matrícula 2024-0002, Password 2024-0002';
PRINT 'DOCENTE:    Cédula 001-1234567-1, Password 00112345671';
PRINT 'DOCENTE:    Cédula 002-2345678-2, Password 00223456782';
PRINT '========================================';
GO
