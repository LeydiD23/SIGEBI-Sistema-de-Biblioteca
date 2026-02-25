using SGA.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Repository
{
    public interface ILibroRepository
    {
        Task<List<Libro>> GetAllAsync();
        Task<Libro?> GetByIdAsync(int id);
        Task AddAsync(Libro libro);
        Task UpdateAsync(Libro libro);
        Task DeleteAsync(int id);
    }
}
