using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EcoSystem.Client.Models;

namespace EcoSystem.Client.Services
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService(IHttpClientFactory httpClientFactory)
        {
            // Este cliente ya viene con el AuthHandler conectado
            _httpClient = httpClientFactory.CreateClient("AuthApi");
        }

        public async Task<bool> CrearProductoAsync(Producto nuevoProducto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Productos", nuevoProducto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Productos");

                if (response.IsSuccessStatusCode)
                {
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