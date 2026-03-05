using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Repository
{
    public interface INotificacionRepository : IBaseRepository<Notificacion>
    {
        Task<IEnumerable<Notificacion>> GetByEstudianteIdAsync(int estudianteId);
        Task<IEnumerable<Notificacion>> GetByDocenteIdAsync(int docenteId);
        Task<IEnumerable<Notificacion>> GetNoLeidasAsync();
        Task MarcarComoLeidaAsync(int id);
    }
}
