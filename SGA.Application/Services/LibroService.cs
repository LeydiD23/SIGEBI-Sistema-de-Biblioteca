using SGA.Application.DTOs;
using SGA.Application.Interfaces;
using SGA.Domain.Enums;
using SGA.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SGA.Application.Services
{
    public class LibroService : ILibroService
    {
        private readonly AppDbContext _context;

        public LibroService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LibroDto>> GetAllAsync()
        {
            var libros = await _context.Libros
                .Include(l => l.Categoria)
                .ToListAsync();

            return libros.Select(MapToDto);
        }

        public async Task<LibroDto> GetByIdAsync(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (libro == null)
                throw new Exception("Libro no encontrado");

            return MapToDto(libro);
        }

        public async Task<LibroDto> CreateAsync(CreateLibroDto dto)
        {
            var libro = new Domain.Entitys.Libro
            {
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                ISBN = dto.ISBN,
                Ubicacion = dto.Ubicacion,
                Editorial = dto.Editorial,
                Stock = dto.Stock,
                StockDisponible = dto.Stock,
                Estado = EstadoRecurso.Disponible,
                FechaAdquisicion = DateTime.Now,
                CategoriaId = dto.CategoriaId
            };

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(libro.Id);
        }

        public async Task<LibroDto> UpdateAsync(UpdateLibroDto dto)
        {
            var libro = await _context.Libros.FindAsync(dto.Id);
            if (libro == null)
                throw new Exception("Libro no encontrado");

            if (libro.Stock != dto.Stock || libro.Estado != (EstadoRecurso)dto.Estado)
            {
                var prestamosActivos = await _context.Prestamos
                    .AnyAsync(p => p.LibroId == dto.Id && p.Estado == EstadoPrestamo.Activo);

                if (prestamosActivos && libro.Stock != dto.Stock)
                    throw new Exception("No se puede modificar el stock de un libro con préstamos activos");
            }

            libro.Titulo = dto.Titulo;
            libro.Autor = dto.Autor;
            libro.ISBN = dto.ISBN;
            libro.Ubicacion = dto.Ubicacion;
            libro.Editorial = dto.Editorial;
            libro.Stock = dto.Stock;
            libro.StockDisponible = dto.StockDisponible;
            libro.Estado = (EstadoRecurso)dto.Estado;
            libro.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(dto.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
                throw new Exception("Libro no encontrado");

            var tienePrestamos = await _context.Prestamos.AnyAsync(p => p.LibroId == id);
            if (tienePrestamos)
            {
                libro.Estado = EstadoRecurso.DadoDeBaja;
                await _context.SaveChangesAsync();
                return true;
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<LibroDto>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var libros = await _context.Libros
                .Include(l => l.Categoria)
                .Where(l => l.Titulo.Contains(searchTerm) ||
                           l.Autor.Contains(searchTerm) ||
                           l.ISBN.Contains(searchTerm))
                .ToListAsync();

            return libros.Select(MapToDto);
        }

        private LibroDto MapToDto(Domain.Entitys.Libro libro)
        {
            return new LibroDto
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                ISBN = libro.ISBN,
                Ubicacion = libro.Ubicacion,
                Editorial = libro.Editorial,
                Stock = libro.Stock,
                StockDisponible = libro.StockDisponible,
                Estado = libro.Estado.ToString(),
                FechaAdquisicion = libro.FechaAdquisicion,
                CategoriaId = libro.CategoriaId,
                CategoriaNombre = libro.Categoria?.Nombre
            };
        }
    }
}
