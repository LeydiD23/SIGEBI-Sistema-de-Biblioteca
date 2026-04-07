using SGA.Application.DTOs;

namespace SGA.Web.Services
{
    public interface IPrestamoApiService
    {
        Task<List<PrestamoDto>?> GetAllAsync();
        Task<PrestamoDto?> GetByIdAsync(int id);
        Task<List<PrestamoDto>?> GetByUsuarioIdAsync(int? estudianteId, int? docenteId);
        Task<bool> DevolverAsync(int id);
    }

    public class PrestamoApiService : ApiService, IPrestamoApiService
    {
        public PrestamoApiService(HttpClient httpClient, IConfiguration configuration) 
            : base(httpClient, configuration) { }

        public async Task<List<PrestamoDto>?> GetAllAsync()
        {
            return await GetListAsync<PrestamoDto>("prestamos");
        }

        public async Task<PrestamoDto?> GetByIdAsync(int id)
        {
            return await GetAsync<PrestamoDto>($"prestamos/{id}");
        }

        public async Task<List<PrestamoDto>?> GetByUsuarioIdAsync(int? estudianteId, int? docenteId)
        {
            var query = new List<string>();
            if (estudianteId.HasValue)
                query.Add($"estudianteId={estudianteId.Value}");
            if (docenteId.HasValue)
                query.Add($"docenteId={docenteId.Value}");
            
            var queryString = query.Count > 0 ? "?" + string.Join("&", query) : "";
            return await GetListAsync<PrestamoDto>($"prestamos/usuario{queryString}");
        }

        public async Task<bool> DevolverAsync(int id)
        {
            return await PutAsync($"prestamos/devolver/{id}");
        }
    }
}
