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

		return builder.Build();
	}
}
