using SGA.Application.DTOs;

namespace SGA.Application.Interfaces
{
    public interface IBibliotecarioService
    {
        Task<IEnumerable<BibliotecarioDto>> GetAllAsync();
        Task<BibliotecarioDto> GetByIdAsync(int id);
        Task<BibliotecarioDto> CreateAsync(CreateBibliotecarioDto dto);
        Task<BibliotecarioDto> UpdateAsync(UpdateBibliotecarioDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
