using SGA.Application.DTOs;
using SGA.Application.Interfaces;
using SGA.Domain.Enums;
using SGA.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SGA.Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly AppDbContext _context;

        public ReservaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReservaDto>> GetAllAsync()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Libro)
                .Include(r => r.Estudiante)
                .Include(r => r.Docente)
                .ToListAsync();

            return reservas.Select(MapToDto);
        }

        public async Task<ReservaDto> GetByIdAsync(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Libro)
                .Include(r => r.Estudiante)
                .Include(r => r.Docente)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                throw new Exception("Reserva no encontrada");

            return MapToDto(reserva);
        }

        public async Task<IEnumerable<ReservaDto>> GetByUsuarioIdAsync(int? estudianteId, int? docenteId)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Libro)
                .Include(r => r.Estudiante)
                .Include(r => r.Docente)
                .Where(r => r.EstudianteId == estudianteId || r.DocenteId == docenteId)
                .ToListAsync();

            return reservas.Select(MapToDto);
        }

        public async Task<ReservaDto> GetByLibroIdAsync(int libroId)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Libro)
                .Include(r => r.Estudiante)
                .Include(r => r.Docente)
                .Where(r => r.LibroId == libroId && r.Estado == EstadoReserva.Pendiente)
                .OrderBy(r => r.FechaReserva)
                .FirstOrDefaultAsync();

            if (reserva == null)
                return null;

            return MapToDto(reserva);
        }

        public async Task<ReservaDto> CreateAsync(CreateReservaDto dto)
        {
            if (dto.EstudianteId.HasValue)
            {
                var estudiante = await _context.Estudiantes.FindAsync(dto.EstudianteId.Value);
                if (estudiante == null || !estudiante.Estado)
                    throw new Exception("Estudiante no encontrado o inactivo");
            }

            if (dto.DocenteId.HasValue)
            {
                var docente = await _context.Docentes.FindAsync(dto.DocenteId.Value);
                if (docente == null || !docente.Estado)
                    throw new Exception("Docente no encontrado o inactivo");
            }

            var libro = await _context.Libros.FindAsync(dto.LibroId);
            if (libro == null)
                throw new Exception("Libro no encontrado");

            if (libro.Estado == EstadoRecurso.Disponible)
                throw new Exception("El libro está disponible, no necesita reserva");

            var reservaExistente = await _context.Reservas
                .AnyAsync(r => r.LibroId == dto.LibroId &&
                              (r.EstudianteId == dto.EstudianteId || r.DocenteId == dto.DocenteId) &&
                              r.Estado == EstadoReserva.Pendiente);

            if (reservaExistente)
                throw new Exception("Ya tienes una reserva activa para este libro");

            var posicionCola = await _context.Reservas
                .CountAsync(r => r.LibroId == dto.LibroId && r.Estado == EstadoReserva.Pendiente) + 1;

            var fechaExpiracion = DateTime.Now.AddDays(3);

            var reserva = new Domain.Entitys.Reserva
            {
                FechaReserva = DateTime.Now,
                FechaExpiracion = fechaExpiracion,
                PosicionCola = posicionCola,
                Estado = EstadoReserva.Pendiente,
                LibroId = dto.LibroId,
                EstudianteId = dto.EstudianteId,
                DocenteId = dto.DocenteId
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            await _context.Entry(reserva).Reference(r => r.Libro).LoadAsync();
            if (reserva.EstudianteId.HasValue)
                await _context.Entry(reserva).Reference(r => r.Estudiante).LoadAsync();
            if (reserva.DocenteId.HasValue)
                await _context.Entry(reserva).Reference(r => r.Docente).LoadAsync();

            return MapToDto(reserva);
        }

        public async Task<ReservaDto> UpdateAsync(UpdateReservaDto dto)
        {
            var reserva = await _context.Reservas.FindAsync(dto.Id);
            if (reserva == null)
                throw new Exception("Reserva no encontrada");

            reserva.FechaReserva = dto.FechaReserva;
            reserva.FechaExpiracion = dto.FechaExpiracion;
            reserva.PosicionCola = dto.PosicionCola;
            reserva.Estado = (EstadoReserva)dto.Estado;
            reserva.LibroId = dto.LibroId;
            reserva.EstudianteId = dto.EstudianteId;
            reserva.DocenteId = dto.DocenteId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(dto.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                throw new Exception("Reserva no encontrada");

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelarAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                throw new Exception("Reserva no encontrada");

            if (reserva.Estado != EstadoReserva.Pendiente)
                throw new Exception("Solo se pueden cancelar reservas pendientes");

            reserva.Estado = EstadoReserva.Cancelada;
            await _context.SaveChangesAsync();

            var reservasRestantes = await _context.Reservas
                .Where(r => r.LibroId == reserva.LibroId && r.Estado == EstadoReserva.Pendiente && r.Id != reserva.Id)
                .OrderBy(r => r.FechaReserva)
                .ToListAsync();

            int posicion = 1;
            foreach (var r in reservasRestantes)
            {
                r.PosicionCola = posicion++;
            }
            await _context.SaveChangesAsync();

            return true;
        }

        private ReservaDto MapToDto(Domain.Entitys.Reserva reserva)
        {
            return new ReservaDto
            {
                Id = reserva.Id,
                FechaReserva = reserva.FechaReserva,
                FechaExpiracion = reserva.FechaExpiracion,
                PosicionCola = reserva.PosicionCola,
                Estado = reserva.Estado.ToString(),
                LibroId = reserva.LibroId,
                LibroTitulo = reserva.Libro?.Titulo,
                EstudianteId = reserva.EstudianteId,
                EstudianteNombre = reserva.Estudiante?.Nombre,
                DocenteId = reserva.DocenteId,
                DocenteNombre = reserva.Docente?.Nombre
            };
        }
    }
}
