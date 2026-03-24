using SGA.Application.DTOs;
using SGA.Application.Interfaces;
using SGA.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SGA.Application.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly AppDbContext _context;

        public EstudianteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EstudianteDto>> GetAllAsync()
        {
            var estudiantes = await _context.Estudiantes.ToListAsync();
            return estudiantes.Select(MapToDto);
        }

        public async Task<EstudianteDto> GetByIdAsync(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
                throw new Exception("Estudiante no encontrado");
            return MapToDto(estudiante);
        }

        public async Task<EstudianteDto> GetByMatriculaAsync(string matricula)
        {
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Matricula == matricula);
            if (estudiante == null)
                throw new Exception("Estudiante no encontrado");
            return MapToDto(estudiante);
        }

        public async Task<EstudianteDto> CreateAsync(CreateEstudianteDto dto)
        {
            var existe = await _context.Estudiantes.AnyAsync(e => e.Matricula == dto.Matricula);
            if (existe)
                throw new Exception("Ya existe un estudiante con esta matrícula");

            var estudiante = new Domain.Entitys.Estudiante
            {
                Nombre = dto.Nombre,
                Matricula = dto.Matricula,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Carrera = dto.Carrera,
                Estado = true
            };

            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            return MapToDto(estudiante);
        }

        public async Task<EstudianteDto> UpdateAsync(UpdateEstudianteDto dto)
        {
            var estudiante = await _context.Estudiantes.FindAsync(dto.Id);
            if (estudiante == null)
                throw new Exception("Estudiante no encontrado");

            estudiante.Nombre = dto.Nombre;
            estudiante.Matricula = dto.Matricula;
            estudiante.Email = dto.Email;
            estudiante.Telefono = dto.Telefono;
            estudiante.Carrera = dto.Carrera;
            estudiante.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return MapToDto(estudiante);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
                throw new Exception("Estudiante no encontrado");

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return true;
        }

        private EstudianteDto MapToDto(Domain.Entitys.Estudiante estudiante)
        {
            return new EstudianteDto
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre,
                Matricula = estudiante.Matricula,
                Email = estudiante.Email,
                Telefono = estudiante.Telefono,
                Carrera = estudiante.Carrera,
                Estado = estudiante.Estado
            };
        }
    }
}
