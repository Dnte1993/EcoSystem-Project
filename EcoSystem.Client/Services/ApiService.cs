using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EcoSystem.Client.Models;

namespace EcoSystem.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            // Usamos tu factory para mantener la autenticación (AuthHandler) de las firmas anteriores
            _httpClient = httpClientFactory.CreateClient("AuthApi");

            // Configurar timeout global a 15 segundos como exige la rúbrica
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            try
            {
                Console.WriteLine("Iniciando petición GET asíncrona...");

                var response = await _httpClient.GetAsync("api/Productos");

                // Lanza excepción si el status code es 4xx o 5xx
                response.EnsureSuccessStatusCode();

                var productos = await response.Content.ReadFromJsonAsync<List<Producto>>();
                var resultado = productos ?? new List<Producto>();

                // Criterio de Aceptación: Imprimir en consola los nombres y datos
                Console.WriteLine($"{resultado.Count} producto(s) recuperado(s) exitosamente:\n");
                foreach (var p in resultado)
                {
                    Console.WriteLine($" [{p.Id}] {p.Nombre} - ${p.Precio:F2} (Stock: {p.Stock})");
                }

                return resultado;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de red: {ex.Message}");
                return new List<Producto>();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Tiempo de espera agotado.");
                return new List<Producto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return new List<Producto>();
            }
        }

        // --- Resto de las operaciones CRUD migradas ---

        public async Task<bool> CrearProductoAsync(Producto nuevoProducto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Productos", nuevoProducto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarProductoAsync(int id, Producto productoModificado)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Productos/{id}", productoModificado);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ActualizarProductoAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarProductoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Productos/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EliminarProductoAsync: {ex.Message}");
                return false;
            }
        }
    }
}