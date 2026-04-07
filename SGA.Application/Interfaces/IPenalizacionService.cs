using SGA.Application.DTOs;

namespace SGA.Application.Interfaces
{
    public interface IPenalizacionService
    {
        Task<IEnumerable<PenalizacionDto>> GetAllAsync();
        Task<PenalizacionDto> GetByIdAsync(int id);
        Task<IEnumerable<PenalizacionDto>> GetByUsuarioIdAsync(int? estudianteId, int? docenteId);
        Task<IEnumerable<PenalizacionDto>> GetActivasAsync(int? estudianteId, int? docenteId);
        Task<PenalizacionDto> CreateAsync(CreatePenalizacionDto dto);
        Task<PenalizacionDto> UpdateAsync(UpdatePenalizacionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> RegistrarPagoAsync(int id);
    }
}
