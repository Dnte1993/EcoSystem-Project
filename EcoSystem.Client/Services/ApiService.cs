using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EcoSystem.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        // Reemplaza esto con la URL real de tu API en Render
        private readonly string _baseUrl = "https://tu-api-ecosystem.onrender.com/api";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        // ----------------------------------------------------------------------
        // INYECCIÓN DE SEGURIDAD 
        // ----------------------------------------------------------------------
        private async Task InyectarTokenAsync()
        {
            // Recuperamos el token que guardaste exitosamente en la Firma 4
            var token = await SecureStorage.Default.GetAsync("jwt_token");

            if (!string.IsNullOrEmpty(token))
            {
                // Se inyecta el token en el encabezado Authorization: Bearer <token>
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ----------------------------------------------------------------------
        // PLANTILLAS PARA EL CRUD 
        // ----------------------------------------------------------------------

        // Método genérico para POST (Crear)
        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            await InyectarTokenAsync(); // Aseguramos la petición

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
        }

        // Método genérico para PUT (Actualizar)
        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            await InyectarTokenAsync();

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PutAsync($"{_baseUrl}/{endpoint}", content);
        }

        // Método genérico para DELETE (Eliminar)
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            await InyectarTokenAsync();

            return await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint}");
        }
    }
}