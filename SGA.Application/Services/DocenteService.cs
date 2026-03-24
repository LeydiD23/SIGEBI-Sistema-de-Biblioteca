using SGA.Application.DTOs;
using SGA.Application.Interfaces;
using SGA.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SGA.Application.Services
{
    public class DocenteService : IDocenteService
    {
        private readonly AppDbContext _context;

        public DocenteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocenteDto>> GetAllAsync()
        {
            var docentes = await _context.Docentes.ToListAsync();
            return docentes.Select(MapToDto);
        }

        public async Task<DocenteDto> GetByIdAsync(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null)
                throw new Exception("Docente no encontrado");
            return MapToDto(docente);
        }

        public async Task<DocenteDto> GetByCedulaAsync(string cedula)
        {
            var docente = await _context.Docentes.FirstOrDefaultAsync(d => d.Cedula == cedula);
            if (docente == null)
                throw new Exception("Docente no encontrado");
            return MapToDto(docente);
        }

        public async Task<DocenteDto> CreateAsync(CreateDocenteDto dto)
        {
            var existe = await _context.Docentes.AnyAsync(d => d.Cedula == dto.Cedula);
            if (existe)
                throw new Exception("Ya existe un docente con esta cédula");

            var docente = new Domain.Entitys.Docente
            {
                Nombre = dto.Nombre,
                Cedula = dto.Cedula,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Departamento = dto.Departamento,
                Estado = true
            };

            _context.Docentes.Add(docente);
            await _context.SaveChangesAsync();

            return MapToDto(docente);
        }

        public async Task<DocenteDto> UpdateAsync(UpdateDocenteDto dto)
        {
            var docente = await _context.Docentes.FindAsync(dto.Id);
            if (docente == null)
                throw new Exception("Docente no encontrado");

            docente.Nombre = dto.Nombre;
            docente.Cedula = dto.Cedula;
            docente.Email = dto.Email;
            docente.Telefono = dto.Telefono;
            docente.Departamento = dto.Departamento;
            docente.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return MapToDto(docente);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null)
                throw new Exception("Docente no encontrado");

            _context.Docentes.Remove(docente);
            await _context.SaveChangesAsync();
            return true;
        }

        private DocenteDto MapToDto(Domain.Entitys.Docente docente)
        {
            return new DocenteDto
            {
                Id = docente.Id,
                Nombre = docente.Nombre,
                Cedula = docente.Cedula,
                Email = docente.Email,
                Telefono = docente.Telefono,
                Departamento = docente.Departamento,
                Estado = docente.Estado
            };
        }
    }
}
