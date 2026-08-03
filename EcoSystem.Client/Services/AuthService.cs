using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using EcoSystem.Client.Models;

namespace EcoSystem.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("AuthApi");
        }

        public async Task<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            var payload = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", payload, cancellationToken);

            return response.StatusCode switch
            {
                HttpStatusCode.OK => new LoginResult
                {
                    Success = true,
                    // CORRECCIÓN: Si el token viene nulo desde la API, asignamos string.Empty
                    Token = (await response.Content.ReadFromJsonAsync<AuthResponse>())?.Token ?? string.Empty
                },
                HttpStatusCode.Unauthorized => new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Usuario o contraseña incorrectos."
                },
                HttpStatusCode.Forbidden => new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Tu cuenta no tiene acceso a esta aplicación."
                },
                HttpStatusCode.InternalServerError => new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Error en el servidor. Inténtalo más tarde."
                },
                _ => new LoginResult
                {
                    Success = false,
                    ErrorMessage = $"Error inesperado: {(int)response.StatusCode}"
                }
            };
        }
    }
}