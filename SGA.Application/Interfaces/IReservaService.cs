using SGA.Application.DTOs;

namespace SGA.Application.Interfaces
{
    public interface IReservaService
    {
        Task<IEnumerable<ReservaDto>> GetAllAsync();
        Task<ReservaDto> GetByIdAsync(int id);
        Task<IEnumerable<ReservaDto>> GetByUsuarioIdAsync(int? estudianteId, int? docenteId);
        Task<ReservaDto> GetByLibroIdAsync(int libroId);
        Task<ReservaDto> CreateAsync(CreateReservaDto dto);
        Task<ReservaDto> UpdateAsync(UpdateReservaDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> CancelarAsync(int id);
    }
}
