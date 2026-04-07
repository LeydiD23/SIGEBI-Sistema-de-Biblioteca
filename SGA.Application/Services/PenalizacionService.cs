using SGA.Application.DTOs;
using SGA.Application.Interfaces;
using SGA.Domain.Enums;
using SGA.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SGA.Application.Services
{
    public class PenalizacionService : IPenalizacionService
    {
        private readonly AppDbContext _context;

        public PenalizacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PenalizacionDto>> GetAllAsync()
        {
            var penalizaciones = await _context.Penalizaciones
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .Include(p => p.Prestamo)
                .ToListAsync();

            return penalizaciones.Select(MapToDto);
        }

        public async Task<PenalizacionDto> GetByIdAsync(int id)
        {
            var penalizacion = await _context.Penalizaciones
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .Include(p => p.Prestamo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (penalizacion == null)
                throw new Exception("Penalización no encontrada");

            return MapToDto(penalizacion);
        }

        public async Task<IEnumerable<PenalizacionDto>> GetByUsuarioIdAsync(int? estudianteId, int? docenteId)
        {
            var penalizaciones = await _context.Penalizaciones
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .Include(p => p.Prestamo)
                .Where(p => p.EstudianteId == estudianteId || p.DocenteId == docenteId)
                .ToListAsync();

            return penalizaciones.Select(MapToDto);
        }

        public async Task<IEnumerable<PenalizacionDto>> GetActivasAsync(int? estudianteId, int? docenteId)
        {
            var penalizaciones = await _context.Penalizaciones
                .Include(p => p.Estudiante)
                .Include(p => p.Docente)
                .Include(p => p.Prestamo)
                .Where(p => p.Estado == EstadoPenalizacion.Activa &&
                           (p.EstudianteId == estudianteId || p.DocenteId == docenteId))
                .ToListAsync();

            return penalizaciones.Select(MapToDto);
        }

        public async Task<PenalizacionDto> CreateAsync(CreatePenalizacionDto dto)
        {
            var penalizacion = new Domain.Entitys.Penalizacion
            {
                Motivo = dto.Motivo,
                Monto = dto.Monto,
                DiasRetraso = dto.DiasRetraso,
                FechaGeneracion = DateTime.Now,
                Estado = EstadoPenalizacion.Pendiente,
                EstudianteId = dto.EstudianteId,
                DocenteId = dto.DocenteId,
                PrestamoId = dto.PrestamoId
            };

            _context.Penalizaciones.Add(penalizacion);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(penalizacion.Id);
        }

        public async Task<PenalizacionDto> UpdateAsync(UpdatePenalizacionDto dto)
        {
            var penalizacion = await _context.Penalizaciones.FindAsync(dto.Id);
            if (penalizacion == null)
                throw new Exception("Penalización no encontrada");

            penalizacion.Motivo = dto.Motivo;
            penalizacion.Monto = dto.Monto;
            penalizacion.DiasRetraso = dto.DiasRetraso;
            penalizacion.FechaGeneracion = dto.FechaGeneracion;
            penalizacion.FechaPago = dto.FechaPago;
            penalizacion.Estado = (EstadoPenalizacion)dto.Estado;
            penalizacion.EstudianteId = dto.EstudianteId;
            penalizacion.DocenteId = dto.DocenteId;
            penalizacion.PrestamoId = dto.PrestamoId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(dto.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var penalizacion = await _context.Penalizaciones.FindAsync(id);
            if (penalizacion == null)
                throw new Exception("Penalización no encontrada");

            _context.Penalizaciones.Remove(penalizacion);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegistrarPagoAsync(int id)
        {
            var penalizacion = await _context.Penalizaciones.FindAsync(id);
            if (penalizacion == null)
                throw new Exception("Penalización no encontrada");

            if (penalizacion.Estado == EstadoPenalizacion.Pagada)
                throw new Exception("La penalización ya fue pagada");

            penalizacion.FechaPago = DateTime.Now;
            penalizacion.Estado = EstadoPenalizacion.Pagada;

            await _context.SaveChangesAsync();

            return true;
        }

        private PenalizacionDto MapToDto(Domain.Entitys.Penalizacion penalizacion)
        {
            return new PenalizacionDto
            {
                Id = penalizacion.Id,
                Motivo = penalizacion.Motivo,
                Monto = penalizacion.Monto,
                DiasRetraso = penalizacion.DiasRetraso,
                FechaGeneracion = penalizacion.FechaGeneracion,
                FechaPago = penalizacion.FechaPago,
                Estado = penalizacion.Estado.ToString(),
                EstudianteId = penalizacion.EstudianteId,
                EstudianteNombre = penalizacion.Estudiante?.Nombre,
                DocenteId = penalizacion.DocenteId,
                DocenteNombre = penalizacion.Docente?.Nombre,
                PrestamoId = penalizacion.PrestamoId
            };
        }
    }
}
