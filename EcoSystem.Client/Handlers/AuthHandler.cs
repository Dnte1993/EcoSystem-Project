using System.Net;
using System.Net.Http.Headers;

namespace EcoSystem.Client.Handlers
{
    public class AuthHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 1. Recuperamos el token de forma asíncrona
            var token = await SecureStorage.Default.GetAsync("jwt_token");

            // 2. Si el token existe, se inyecta automáticamente en la cabecera
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // 3. Enviamos la petición al servidor (Render)
            var response = await base.SendAsync(request, cancellationToken);

            // 4. Si el servidor responde 401 (No Autorizado), el token expiró o es inválido
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Limpiamos las credenciales almacenadas
                SecureStorage.Default.RemoveAll();

                // Redirigimos al flujo de login de forma centralizada en el hilo principal
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                });
            }

            return response;
        }
    }
}