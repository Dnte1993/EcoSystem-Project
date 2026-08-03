namespace EcoSystem.Client;

using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using EcoSystem.Client.Views;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Rutas registradas para navegación interna (ocultas del menú)
		Routing.RegisterRoute(nameof(Views.NuevoProductoPage), typeof(Views.NuevoProductoPage));

		// NUEVA: Ruta para que el Login pueda navegar al Registro
		Routing.RegisterRoute(nameof(Views.RegistroUsuarioPage), typeof(Views.RegistroUsuarioPage));
	}

	// CORRECCIÓN: Agregamos el '?' en object? sender
	private async void OnCerrarSesionClicked(object? sender, EventArgs e)
	{
		// 1. Borramos el token y su fecha de expiración usando las llaves 
		// exactas que definiste en tu SecureTokenService.cs
		SecureStorage.Default.Remove("auth_token");
		SecureStorage.Default.Remove("token_expiry");

		// 2. Redirigimos al usuario a la pantalla de Login
		await Current.GoToAsync("//LoginPage");
	}
}