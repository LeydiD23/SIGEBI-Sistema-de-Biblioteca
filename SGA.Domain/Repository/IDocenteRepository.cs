using SGA.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Repository
{
    public interface IDocenteRepository : IBaseRepository<Docente>
    {
        Task<Docente?> GetByCedulaAsync(string cedula);
        Task<IEnumerable<Docente>> GetDocentesActivosAsync();
        Task<Docente?> GetByEmailAsync(string email);
    }
}
