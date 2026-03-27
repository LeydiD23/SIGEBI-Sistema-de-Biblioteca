using SGA.Application.DTOs;

namespace SGA.Application.Interfaces
{
    public interface IPrestamoService
    {
        Task<IEnumerable<PrestamoDto>> GetAllAsync();
        Task<PrestamoDto> GetByIdAsync(int id);
        Task<IEnumerable<PrestamoDto>> GetByUsuarioIdAsync(int? estudianteId, int? docenteId);
        Task<PrestamoDto> CreateAsync(CreatePrestamoDto dto);
        Task<PrestamoDto> UpdateAsync(UpdatePrestamoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<PrestamoDto> DevolverAsync(int id);
        Task<IEnumerable<PrestamoDto>> GetPrestamosVencidosAsync();
    }
}
