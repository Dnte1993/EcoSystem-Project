using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EcoSystem.Client.Models;
using EcoSystem.Client.Services;

namespace EcoSystem.Client.ViewModels
{
    public class ListaProductosViewModel : INotifyPropertyChanged
    {
        private readonly ProductoService _productoService;

        // ObservableCollection es clave: actualiza la UI automáticamente al recibir datos
        public ObservableCollection<Producto> Productos { get; } = new ObservableCollection<Producto>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public ICommand CargarProductosCommand { get; }

        public ListaProductosViewModel(ProductoService productoService)
        {
            _productoService = productoService;
            CargarProductosCommand = new Command(async () => await CargarProductosAsync());
        }

        public async Task CargarProductosAsync()
        {
            // Si ya está ejecutándose la carga, evitamos peticiones duplicadas
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                // Limpiamos la lista local antes de traer la nueva información
                Productos.Clear();

                // Llamamos al servicio GET que creamos en el paso anterior
                var productosDesdeApi = await _productoService.GetProductosAsync();

                // Llenamos la colección uno por uno para que la vista se entere
                foreach (var prod in productosDesdeApi)
                {
                    Productos.Add(prod);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudieron cargar los productos: {ex.Message}", "OK");
            }
            finally
            {
                // Liberamos el estado de carga siempre, pase lo que pase
                IsBusy = false;
            }
        }

        // --- Implementación de INotifyPropertyChanged ---
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}