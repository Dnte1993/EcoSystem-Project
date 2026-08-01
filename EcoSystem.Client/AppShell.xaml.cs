namespace EcoSystem.Client;

using EcoSystem.Client.Views;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(Views.NuevoProductoPage), typeof(Views.NuevoProductoPage));
	}
}
