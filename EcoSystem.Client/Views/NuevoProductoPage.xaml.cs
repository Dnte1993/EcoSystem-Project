 using Microsoft.Maui.Controls;
using EcoSystem.Client.ViewModels;

namespace EcoSystem.Client.Views
{
    public partial class NuevoProductoPage : ContentPage
    {
        // Inyectamos el ViewModel
        public NuevoProductoPage(NuevoProductoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; // Enlazamos la UI con nuestra lógica
        }
    }
}