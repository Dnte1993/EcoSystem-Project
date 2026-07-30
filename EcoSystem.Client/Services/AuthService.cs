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
            // Solicitamos el cliente HTTP configurado desde el contenedor
            _httpClient = factory.CreateClient("AuthApi");
        }

        public async Task<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            var payload = new LoginRequest
            {
                Username = username,
                Password = password
            };

            // Se envía por POST en formato JSON como exige la rúbrica
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", payload, cancellationToken);

            return response.StatusCode switch
            {
                HttpStatusCode.OK => new LoginResult
                {
                    Success = true,
                    Token = (await response.Content.ReadFromJsonAsync<AuthResponse>())?.Token
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