using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace EcoSystem.Client;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	// --- NUEVO: Sobrescribimos el ciclo de vida de arranque de la app ---
	protected override async void OnStart()
	{
		base.OnStart();
		// Llamamos a nuestro método de seguridad al arrancar
		await CheckAndRestoreSessionAsync();
	}

	// --- NUEVO: Método de validación exigido por la rúbrica Firma 4 ---
	private async Task CheckAndRestoreSessionAsync()
	{
		try
		{
			// 1. Leer token del almacenamiento seguro
			var token = await SecureStorage.GetAsync("jwt_token");

			// 2. Si no existe token -> al login
			if (string.IsNullOrWhiteSpace(token))
			{
				await Shell.Current.GoToAsync("//LoginPage");
				return;
			}

			// 3. Leer y parsear la fecha de expiración
			var expStr = await SecureStorage.GetAsync("jwt_exp");

			if (!DateTime.TryParse(expStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expDate))
			{
				// Formato inválido -> tratar como expirado
				await Shell.Current.GoToAsync("//LoginPage");
				return;
			}

			// 4. Verificar si el token ha expirado (margen de 60 segundos)
			if (expDate < DateTime.UtcNow.AddSeconds(60))
			{
				// Token expirado -> limpiar y redirigir
				SecureStorage.Default.RemoveAll();
				await Shell.Current.GoToAsync("//LoginPage");
				return;
			}

			// 5. Sesión válida -> navegar a la página principal del Inventario
			await Shell.Current.GoToAsync("//ListaProductosPage");
		}
		catch (Exception ex)
		{
			// En caso de error inesperado, fallar de forma segura
			Debug.WriteLine($"Session restore error: {ex.Message}");
			await Shell.Current.GoToAsync("//LoginPage");
		}
	}
}