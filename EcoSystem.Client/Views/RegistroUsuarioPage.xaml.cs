using System;
using Microsoft.Maui.Controls;
using EcoSystem.Client.ViewModels; // Importamos la carpeta ViewModels

namespace EcoSystem.Client.Views
{
    public partial class RegistroUsuarioPage : ContentPage
    {
        public RegistroUsuarioPage()
        {
            InitializeComponent();

            // Aquí conectamos esta pantalla visual con su "Cerebro"
            BindingContext = new RegistroUsuarioViewModel();
        }

        private async void OnCancelarClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}