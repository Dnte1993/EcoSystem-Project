using Microsoft.Extensions.Logging;
using EcoSystem.Client.ViewModels;
using EcoSystem.Client.Views;
using EcoSystem.Client.Services;

namespace EcoSystem.Client;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// ---> Registro de Dependencias <---
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddSingleton<ITokenService, SecureTokenService>();

		// ---> NUEVO: Configuración del HttpClient para tu API <---
		builder.Services.AddHttpClient("AuthApi", client =>
		{
			// Cambia esta URL por la dirección real donde está alojada tu API
			client.BaseAddress = new Uri("https://ecosystem-project-p0c1.onrender.com");
		});

		// ---> NUEVO: Registro de AuthService <---
		builder.Services.AddTransient<AuthService>();

		return builder.Build();
	}
}