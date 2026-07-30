using Microsoft.Maui.Controls;
using EcoSystem.Client.ViewModels;

namespace EcoSystem.Client.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();

            // Aquí ocurre la magia: conectamos la interfaz con la lógica
            BindingContext = viewModel;
        }
        // Este evento manda la contraseña al ViewModel manualmente por seguridad
        private void txtContrasena_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is LoginViewModel vm)
            {
                vm.Password = e.NewTextValue;
            }
        }
    }
}