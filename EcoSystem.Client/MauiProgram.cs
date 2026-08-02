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
		builder.Services.AddTransient<NuevoProductoViewModel>();
		builder.Services.AddTransient<NuevoProductoPage>();

		builder.Services.AddTransient<ListaProductosViewModel>();
		builder.Services.AddTransient<ListaProductosPage>();

		// ---> NUEVO: Registramos el interceptor que acabas de crear <---
		builder.Services.AddTransient<Handlers.AuthHandler>();

		// ---> MODIFICADO: Configuración del HttpClient conectado al AuthHandler <---
		builder.Services.AddHttpClient("AuthApi", client =>
		{
			// Apuntamos a tu API alojada en Render
			client.BaseAddress = new Uri("https://ecosystem-project-p0c1.onrender.com");
		})
		.AddHttpMessageHandler<Handlers.AuthHandler>(); // ¡Esta línea es el pegamento mágico!

		// ---> Registro de Servicios <---
		builder.Services.AddTransient<AuthService>();
		builder.Services.AddTransient<ProductoService>();

		return builder.Build();
	}
}