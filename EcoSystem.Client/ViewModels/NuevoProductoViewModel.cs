using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EcoSystem.Client.Models;
using EcoSystem.Client.Services;

namespace EcoSystem.Client.ViewModels
{
    public class NuevoProductoViewModel : INotifyPropertyChanged
    {
        private readonly ProductoService _productoService;

        private string _nombre = string.Empty;
        public string Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre == value) return;
                _nombre = value;
                OnPropertyChanged();
            }
        }

        private decimal _precio;
        public decimal Precio
        {
            get => _precio;
            set
            {
                if (_precio == value) return;
                _precio = value;
                OnPropertyChanged();
            }
        }

        private int _stock;
        public int Stock
        {
            get => _stock;
            set
            {
                if (_stock == value) return;
                _stock = value;
                OnPropertyChanged();
            }
        }

        // Variable para controlar el indicador de carga en la UI
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

        public ICommand GuardarCommand { get; }

        // Inyectamos nuestro nuevo ProductoService
        public NuevoProductoViewModel(ProductoService productoService)
        {
            _productoService = productoService;
            GuardarCommand = new Command(async () => await EjecutarGuardarAsync());
        }

        private async Task EjecutarGuardarAsync()
        {
            // 1. Validación de datos locales
            if (string.IsNullOrWhiteSpace(Nombre) || Precio <= 0 || Stock < 0)
            {
                await Application.Current.MainPage.DisplayAlert("Validación", "Ingresa un nombre válido, un precio mayor a 0 y un stock válido.", "OK");
                return;
            }

            // 2. Activamos el estado de carga (deshabilitará el botón en la vista)
            IsBusy = true;

            try
            {
                // Construimos el objeto a enviar
                var nuevoProducto = new Producto
                {
                    Nombre = Nombre,
                    Precio = Precio,
                    Stock = Stock
                };

                // 3. Llamamos a nuestro servicio
                bool exito = await _productoService.CrearProductoAsync(nuevoProducto);

                if (exito)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Producto creado correctamente en la nube.", "OK");

                    // Limpiamos los campos tras el éxito
                    Nombre = string.Empty;
                    Precio = 0;
                    Stock = 0;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear. Verifica que tu sesión siga activa.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error inesperado", ex.Message, "OK");
            }
            finally
            {
                // 4. Apagamos el estado de carga pase lo que pase
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