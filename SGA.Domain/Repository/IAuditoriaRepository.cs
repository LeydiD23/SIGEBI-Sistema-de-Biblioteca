using SGA.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Repository
{
    public interface IAuditoriaRepository : IBaseRepository<Auditoria>
    {
        Task<IEnumerable<Auditoria>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Auditoria>> GetByEntidadAsync(string entidad);
        Task<IEnumerable<Auditoria>> GetByFechaAsync(DateTime fecha);
    }
}
