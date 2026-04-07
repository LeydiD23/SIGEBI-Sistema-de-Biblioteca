namespace SGA.Web.Services
{
    public interface IAuthService
    {
        Task<LoginResultDto?> LoginAsync(string tipoUsuario, string identificador, string password);
    }

    public class LoginResultDto
    {
        public bool Success { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = "";
        public string Rol { get; set; } = "";
        public string TipoUsuario { get; set; } = "";
    }

    public class AuthService : ApiService, IAuthService
    {
        public AuthService(HttpClient httpClient, IConfiguration configuration) 
            : base(httpClient, configuration) { }

        public async Task<LoginResultDto?> LoginAsync(string tipoUsuario, string identificador, string password)
        {
            var endpoint = tipoUsuario == "estudiante" ? "estudiantes" : "docentes";
            var param = tipoUsuario == "estudiante" ? "matricula" : "cedula";
            
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}/login?{param}={Uri.EscapeDataString(identificador)}&password={Uri.EscapeDataString(password)}");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResultDto>(_jsonOptions);
                }
                
                return new LoginResultDto { Success = false };
            }
            catch
            {
                return new LoginResultDto { Success = false };
            }
        }
    }
}
