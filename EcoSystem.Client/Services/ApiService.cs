namespace EcoSystem.Client.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
        // Métodos GET/POST irán aquí en Firma 2
    }
}