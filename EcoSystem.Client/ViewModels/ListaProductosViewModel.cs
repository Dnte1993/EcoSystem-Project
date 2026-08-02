using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EcoSystem.Client.Models;
using EcoSystem.Client.Services;
using System.Collections.Generic; // NUEVO: Necesario para pasar parámetros de navegación

namespace EcoSystem.Client.ViewModels
{
    public class ListaProductosViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

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

        // NUEVO: Definición de los comandos de acción
        public ICommand EditarCommand { get; }
        public ICommand EliminarCommand { get; }

        public ListaProductosViewModel(ApiService apiService)
        {
            _apiService = apiService;
            CargarProductosCommand = new Command(async () => await CargarProductosAsync());

            // NUEVO: Inicializar los comandos
            EditarCommand = new Command<Producto>(OnEditar);
            EliminarCommand = new Command<Producto>(OnEliminar);
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
                var productosDesdeApi = await _apiService.GetProductosAsync();

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

        // NUEVO: Lógica de Edición
        private async void OnEditar(Producto producto)
        {
            if (producto == null) return;

            // Navegamos al formulario pasándole el objeto completo
            var navigationParameter = new Dictionary<string, object>
            {
                { "ProductoSeleccionado", producto }
            };

            await Shell.Current.GoToAsync("NuevoProductoPage", navigationParameter);
        }

        // NUEVO: Lógica de Eliminación
        private async void OnEliminar(Producto producto)
        {
            if (producto == null) return;

            // 1. Confirmación obligatoria según la rúbrica
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Confirmar Eliminación",
                $"¿Estás seguro de que deseas eliminar '{producto.Nombre}'?",
                "Sí, eliminar",
                "Cancelar");

            // Si el usuario cancela, detenemos el proceso
            if (!respuesta) return;

            IsBusy = true;

            try
            {
                // 2. Llamada a la API
                bool exito = await _apiService.EliminarProductoAsync(producto.Id);

                if (exito)
                {
                    // 3. Actualización de estado local: Removemos el producto de la ObservableCollection
                    // El elemento desaparecerá del listado de forma inmediata[cite: 1, 2].
                    Productos.Remove(producto);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar el producto en la base de datos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un problema: {ex.Message}", "OK");
            }
            finally
            {
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