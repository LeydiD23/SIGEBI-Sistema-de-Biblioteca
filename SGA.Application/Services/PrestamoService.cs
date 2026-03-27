using SGA.Application.DTOs;
using SGA.Application.Interfaces;
using SGA.Domain.Enums;
using SGA.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SGA.Application.Services
{
    public class PrestamoService : IPrestamoService
    {
        private readonly AppDbContext _context;

        public PrestamoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PrestamoDto>> GetAllAsync()
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .ToListAsync();

            return prestamos.Select(MapToDto);
        }

        public async Task<PrestamoDto> GetByIdAsync(int id)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
                throw new Exception("Préstamo no encontrado");

            return MapToDto(prestamo);
        }

        public async Task<IEnumerable<PrestamoDto>> GetByUsuarioIdAsync(int? estudianteId, int? docenteId)
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .Where(p => p.EstudianteId == estudianteId || p.DocenteId == docenteId)
                .ToListAsync();

            return prestamos.Select(MapToDto);
        }

        public async Task<PrestamoDto> CreateAsync(CreatePrestamoDto dto)
        {
            var libro = await _context.Libros.FindAsync(dto.LibroId);
            if (libro == null)
                throw new Exception("Libro no encontrado");
            
            if (libro.Estado != EstadoRecurso.Disponible)
                throw new Exception("El libro no está disponible para préstamo");

            int? estudianteId = dto.EstudianteId;
            int? docenteId = dto.DocenteId;

            if (estudianteId.HasValue)
            {
                var estudiante = await _context.Estudiantes.FindAsync(estudianteId.Value);
                if (estudiante == null || !estudiante.Estado)
                    throw new Exception("Estudiante no encontrado o inactivo");
            }

            if (docenteId.HasValue)
            {
                var docente = await _context.Docentes.FindAsync(docenteId.Value);
                if (docente == null || !docente.Estado)
                    throw new Exception("Docente no encontrado o inactivo");
            }

            var prestamosVencidos = await _context.Prestamos
                .Where(p => p.Estado == EstadoPrestamo.Vencido &&
                           (p.EstudianteId == estudianteId || p.DocenteId == docenteId))
                .ToListAsync();

            if (prestamosVencidos.Any())
                throw new Exception("El usuario tiene préstamos vencidos");

            var tienePenalizacionActiva = await _context.Penalizaciones
                .AnyAsync(p => p.Estado == EstadoPenalizacion.Activa &&
                              (p.EstudianteId == estudianteId || p.DocenteId == docenteId));

            if (tienePenalizacionActiva)
                throw new Exception("El usuario tiene penalizaciones activas");

            int prestamosActivos = await _context.Prestamos
                .CountAsync(p => p.Estado == EstadoPrestamo.Activo &&
                                 (p.EstudianteId == estudianteId || p.DocenteId == docenteId));

            int limitePrestamo = estudianteId.HasValue ? 3 : 5;

            if (prestamosActivos >= limitePrestamo)
                throw new Exception($"El usuario ha alcanzado el límite de {limitePrestamo} préstamos simultáneos");

            int diasPlazo = estudianteId.HasValue ? 7 : 15;
            var fechaLimite = DateTime.Now.AddDays(diasPlazo);

            var prestamo = new Domain.Entitys.Prestamo
            {
                FechaPrestamo = DateTime.Now,
                FechaLimite = fechaLimite,
                Estado = EstadoPrestamo.Activo,
                Renovaciones = 0,
                LibroId = dto.LibroId,
                EstudianteId = dto.EstudianteId,
                DocenteId = dto.DocenteId
            };

            libro.Estado = EstadoRecurso.Prestado;

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            await _context.Entry(prestamo).Reference(p => p.Libro).LoadAsync();
            if (prestamo.EstudianteId.HasValue)
                await _context.Entry(prestamo).Reference(p => p.Estudiante).LoadAsync();
            if (prestamo.DocenteId.HasValue)
                await _context.Entry(prestamo).Reference(p => p.Docente).LoadAsync();

            return MapToDto(prestamo);
        }

        public async Task<PrestamoDto> UpdateAsync(UpdatePrestamoDto dto)
        {
            var prestamo = await _context.Prestamos.FindAsync(dto.Id);
            if (prestamo == null)
                throw new Exception("Préstamo no encontrado");

            prestamo.FechaPrestamo = dto.FechaPrestamo;
            prestamo.FechaLimite = dto.FechaLimite;
            prestamo.FechaDevolucionReal = dto.FechaDevolucionReal;
            prestamo.Estado = (EstadoPrestamo)dto.Estado;
            prestamo.Renovaciones = dto.Renovaciones;
            prestamo.LibroId = dto.LibroId;
            prestamo.EstudianteId = dto.EstudianteId;
            prestamo.DocenteId = dto.DocenteId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(dto.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
                throw new Exception("Préstamo no encontrado");

            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PrestamoDto> DevolverAsync(int id)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
                throw new Exception("Préstamo no encontrado");

            if (prestamo.Estado == EstadoPrestamo.Devuelto)
                throw new Exception("El préstamo ya fue devuelto");

            prestamo.FechaDevolucionReal = DateTime.Now;
            prestamo.Estado = EstadoPrestamo.Devuelto;

            if (prestamo.Libro != null)
            {
                prestamo.Libro.Estado = EstadoRecurso.Disponible;
            }

            if (prestamo.FechaLimite < DateTime.Now)
            {
                var diasRetraso = (int)(DateTime.Now - prestamo.FechaLimite).TotalDays;
                var montoPenalizacion = diasRetraso * 10.00m;

                var penalizacion = new Domain.Entitys.Penalizacion
                {
                    Motivo = $"Retraso en devolución de {diasRetraso} días",
                    Monto = montoPenalizacion,
                    DiasRetraso = diasRetraso,
                    FechaGeneracion = DateTime.Now,
                    Estado = EstadoPenalizacion.Activa,
                    EstudianteId = prestamo.EstudianteId,
                    DocenteId = prestamo.DocenteId,
                    PrestamoId = prestamo.Id
                };

                _context.Penalizaciones.Add(penalizacion);
            }

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<IEnumerable<PrestamoDto>> GetPrestamosVencidosAsync()
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .Where(p => p.Estado == EstadoPrestamo.Activo && p.FechaLimite < DateTime.Now)
                .ToListAsync();

            foreach (var prestamo in prestamos)
            {
                prestamo.Estado = EstadoPrestamo.Vencido;
            }
            await _context.SaveChangesAsync();

            return prestamos.Select(MapToDto);
        }

        private PrestamoDto MapToDto(Domain.Entitys.Prestamo prestamo)
        {
            return new PrestamoDto
            {
                Id = prestamo.Id,
                FechaPrestamo = prestamo.FechaPrestamo,
                FechaLimite = prestamo.FechaLimite,
                FechaDevolucionReal = prestamo.FechaDevolucionReal,
                Estado = prestamo.Estado.ToString(),
                Renovaciones = prestamo.Renovaciones,
                LibroId = prestamo.LibroId,
                LibroTitulo = prestamo.Libro?.Titulo,
                EstudianteId = prestamo.EstudianteId,
                EstudianteNombre = prestamo.Estudiante?.Nombre,
                DocenteId = prestamo.DocenteId,
                DocenteNombre = prestamo.Docente?.Nombre
            };
        }
    }
}
