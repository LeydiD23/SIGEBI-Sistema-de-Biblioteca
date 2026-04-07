using SGA.Application.DTOs;

namespace SGA.Application.Interfaces
{
    public interface ILibroService
    {
        Task<IEnumerable<LibroDto>> GetAllAsync();
        Task<LibroDto> GetByIdAsync(int id);
        Task<LibroDto> CreateAsync(CreateLibroDto dto);
        Task<LibroDto> UpdateAsync(UpdateLibroDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<LibroDto>> SearchAsync(string searchTerm);
    }
}
