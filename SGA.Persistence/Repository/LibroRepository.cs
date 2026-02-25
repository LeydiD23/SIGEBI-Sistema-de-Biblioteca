using SGA.Domain.Entitys;
using SGA.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Persistence.Repository
{
    public class LibroRepository : ILibroRepository
    {
        private static List<Libro> _libros = new List<Libro>();

        public Task<List<Libro>> GetAllAsync()
            => Task.FromResult(_libros);

        public Task<Libro?> GetByIdAsync(int id)
            => Task.FromResult(_libros.FirstOrDefault(x => x.Id == id));

        public Task AddAsync(Libro libro)
        {
            _libros.Add(libro);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Libro libro)
        {
            var existente = _libros.FirstOrDefault(x => x.Id == libro.Id);

            if (existente != null)
            {
                existente.Titulo = libro.Titulo;
                existente.Autor = libro.Autor;
                existente.Stock = libro.Stock;
                existente.CategoriaId = libro.CategoriaId;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var libro = _libros.FirstOrDefault(x => x.Id == id);
            if (libro != null)
                _libros.Remove(libro);

            return Task.CompletedTask;
        }
    }
}
