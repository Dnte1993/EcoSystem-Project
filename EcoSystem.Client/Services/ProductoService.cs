using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EcoSystem.Client.Models;

namespace EcoSystem.Client.Services
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public ProductoService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            // Se conecta al cliente HTTP que registraste en MauiProgram
            _httpClient = httpClientFactory.CreateClient("AuthApi");
            _tokenService = tokenService;
        }

        public async Task<bool> CrearProductoAsync(Producto nuevoProducto)
        {
            // Obtenemos el JWT almacenado
            string token = await _tokenService.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Hacemos el POST al endpoint exacto de tu API
            var response = await _httpClient.PostAsJsonAsync("api/Productos", nuevoProducto);

            return response.IsSuccessStatusCode;
        }
    }
}