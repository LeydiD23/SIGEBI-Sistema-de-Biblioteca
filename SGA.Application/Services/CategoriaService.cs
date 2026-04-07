using SGA.Application.DTOs;
using SGA.Application.Interfaces;
using SGA.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SGA.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return categorias.Select(MapToDto);
        }

        public async Task<CategoriaDto> GetByIdAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                throw new Exception("Categoría no encontrada");
            return MapToDto(categoria);
        }

        public async Task<CategoriaDto> CreateAsync(CreateCategoriaDto dto)
        {
            var categoria = new Domain.Entitys.Categoria
            {
                Nombre = dto.Nombre
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return MapToDto(categoria);
        }

        public async Task<CategoriaDto> UpdateAsync(UpdateCategoriaDto dto)
        {
            var categoria = await _context.Categorias.FindAsync(dto.Id);
            if (categoria == null)
                throw new Exception("Categoría no encontrada");

            categoria.Nombre = dto.Nombre;
            await _context.SaveChangesAsync();

            return MapToDto(categoria);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                throw new Exception("Categoría no encontrada");

            var tieneLibros = await _context.Libros.AnyAsync(l => l.CategoriaId == id);
            if (tieneLibros)
                throw new Exception("No se puede eliminar una categoría que tiene libros asociados");

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return true;
        }

        private CategoriaDto MapToDto(Domain.Entitys.Categoria categoria)
        {
            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }
    }
}
