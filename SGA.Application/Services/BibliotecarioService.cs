using SGA.Application.DTOs;
using SGA.Application.Interfaces;
using SGA.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SGA.Application.Services
{
    public class BibliotecarioService : IBibliotecarioService
    {
        private readonly AppDbContext _context;

        public BibliotecarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BibliotecarioDto>> GetAllAsync()
        {
            var bibliotecarios = await _context.Bibliotecarios.ToListAsync();
            return bibliotecarios.Select(MapToDto);
        }

        public async Task<BibliotecarioDto> GetByIdAsync(int id)
        {
            var bibliotecario = await _context.Bibliotecarios.FindAsync(id);
            if (bibliotecario == null)
                throw new Exception("Bibliotecario no encontrado");
            return MapToDto(bibliotecario);
        }

        public async Task<BibliotecarioDto> CreateAsync(CreateBibliotecarioDto dto)
        {
            var bibliotecario = new Domain.Entitys.Bibliotecario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Cargo = dto.Cargo,
                Estado = true
            };

            _context.Bibliotecarios.Add(bibliotecario);
            await _context.SaveChangesAsync();

            return MapToDto(bibliotecario);
        }

        public async Task<BibliotecarioDto> UpdateAsync(UpdateBibliotecarioDto dto)
        {
            var bibliotecario = await _context.Bibliotecarios.FindAsync(dto.Id);
            if (bibliotecario == null)
                throw new Exception("Bibliotecario no encontrado");

            bibliotecario.Nombre = dto.Nombre;
            bibliotecario.Email = dto.Email;
            bibliotecario.Telefono = dto.Telefono;
            bibliotecario.Cargo = dto.Cargo;
            bibliotecario.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return MapToDto(bibliotecario);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bibliotecario = await _context.Bibliotecarios.FindAsync(id);
            if (bibliotecario == null)
                throw new Exception("Bibliotecario no encontrado");

            _context.Bibliotecarios.Remove(bibliotecario);
            await _context.SaveChangesAsync();
            return true;
        }

        private BibliotecarioDto MapToDto(Domain.Entitys.Bibliotecario bibliotecario)
        {
            return new BibliotecarioDto
            {
                Id = bibliotecario.Id,
                Nombre = bibliotecario.Nombre,
                Email = bibliotecario.Email,
                Telefono = bibliotecario.Telefono,
                Cargo = bibliotecario.Cargo,
                Estado = bibliotecario.Estado
            };
        }
    }
}
