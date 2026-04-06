using SGA.Application.DTOs;

namespace SGA.Web.Services
{
    public interface IReservaApiService
    {
        Task<List<ReservaDto>?> GetAllAsync();
        Task<ReservaDto?> GetByIdAsync(int id);
        Task<List<ReservaDto>?> GetByUsuarioIdAsync(int? estudianteId, int? docenteId);
        Task<ReservaDto?> CreateAsync(CreateReservaDto dto);
        Task<bool> CancelarAsync(int id);
    }

    public class ReservaApiService : ApiService, IReservaApiService
    {
        public ReservaApiService(HttpClient httpClient, IConfiguration configuration) 
            : base(httpClient, configuration) { }

        public async Task<List<ReservaDto>?> GetAllAsync()
        {
            return await GetListAsync<ReservaDto>("reservas");
        }

        public async Task<ReservaDto?> GetByIdAsync(int id)
        {
            return await GetAsync<ReservaDto>($"reservas/{id}");
        }

        public async Task<List<ReservaDto>?> GetByUsuarioIdAsync(int? estudianteId, int? docenteId)
        {
            var query = new List<string>();
            if (estudianteId.HasValue)
                query.Add($"estudianteId={estudianteId.Value}");
            if (docenteId.HasValue)
                query.Add($"docenteId={docenteId.Value}");
            
            var queryString = query.Count > 0 ? "?" + string.Join("&", query) : "";
            return await GetListAsync<ReservaDto>($"reservas/usuario{queryString}");
        }

        public async Task<ReservaDto?> CreateAsync(CreateReservaDto dto)
        {
            return await PostAsync<CreateReservaDto, ReservaDto>("reservas", dto);
        }

        public async Task<bool> CancelarAsync(int id)
        {
            return await PutAsync($"reservas/cancelar/{id}");
        }
    }
}
