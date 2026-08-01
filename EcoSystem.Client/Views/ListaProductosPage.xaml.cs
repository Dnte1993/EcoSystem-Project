using Microsoft.Maui.Controls;
using EcoSystem.Client.ViewModels;

namespace EcoSystem.Client.Views
{
    public partial class ListaProductosPage : ContentPage
    {
        private readonly ListaProductosViewModel _viewModel;

        public ListaProductosPage(ListaProductosViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            // Conectamos la vista con el ViewModel
            BindingContext = _viewModel;
        }

        // Este método se ejecuta automáticamente cuando la pantalla se hace visible
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Disparamos la petición GET a tu API
            if (_viewModel.CargarProductosCommand.CanExecute(null))
            {
                _viewModel.CargarProductosCommand.Execute(null);
            }
        }
    }
}