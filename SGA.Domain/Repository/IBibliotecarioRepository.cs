using SGA.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Repository
{
    public interface IBibliotecarioRepository : IBaseRepository<Bibliotecario>
    {
        Task<Bibliotecario?> GetByEmailAsync(string email);
        Task<IEnumerable<Bibliotecario>> GetActivosAsync();
    }
}
