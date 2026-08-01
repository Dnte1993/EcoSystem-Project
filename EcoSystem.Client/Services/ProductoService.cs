using System;
using System.Collections.Generic;
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

        // --- NUEVA FIRMA 6: MÉTODO GET ---
        public async Task<List<Producto>> GetProductosAsync()
        {
            try
            {
                string token = await _tokenService.GetTokenAsync();

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Hacemos el GET al endpoint
                var response = await _httpClient.GetAsync("api/Productos");

                if (response.IsSuccessStatusCode)
                {
                    // Deserializamos el JSON de tu API a una lista en C#
                    var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();
                    return productos ?? new List<Producto>();
                }
                else
                {
                    throw new Exception($"Error al obtener productos: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetProductosAsync: {ex.Message}");
                throw;
            }
        }
    }
}