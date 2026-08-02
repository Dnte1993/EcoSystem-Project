using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EcoSystem.Client.Models;
using EcoSystem.Client.Services;

namespace EcoSystem.Client.ViewModels
{
    public class NuevoProductoViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private readonly ApiService _apiService;

        private Producto _productoEnEdicion;

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

        public NuevoProductoViewModel(ApiService apiService)
        {
            _apiService = apiService;
            GuardarCommand = new Command(async () => await EjecutarGuardarAsync());
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("ProductoSeleccionado") && query["ProductoSeleccionado"] is Producto producto)
            {
                _productoEnEdicion = producto;
                Nombre = producto.Nombre;
                Precio = producto.Precio;
                Stock = producto.Stock;
            }
        }

        private async Task EjecutarGuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(Nombre) || Precio <= 0 || Stock < 0)
            {
                await Application.Current.MainPage.DisplayAlert("Validación", "Ingresa un nombre válido, un precio mayor a 0 y un stock válido.", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                bool exito = false;

                if (_productoEnEdicion == null)
                {
                    // MODO CREACIÓN (POST)
                    var nuevoProducto = new Producto
                    {
                        Nombre = Nombre,
                        Precio = Precio,
                        Stock = Stock
                    };

                    exito = await _apiService.CrearProductoAsync(nuevoProducto);

                    if (exito)
                    {
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Producto creado correctamente.", "OK");
                    }
                }
                else
                {
                    // MODO EDICIÓN (PUT)
                    _productoEnEdicion.Nombre = Nombre;
                    _productoEnEdicion.Precio = Precio;
                    _productoEnEdicion.Stock = Stock;

                    exito = await _apiService.ActualizarProductoAsync(_productoEnEdicion.Id, _productoEnEdicion);

                    if (exito)
                    {
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Producto actualizado correctamente.", "OK");
                    }
                }

                if (exito)
                {
                    _productoEnEdicion = null;
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo guardar en la nube. Verifica tu conexión.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error inesperado", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // AQUI ESTÁN LOS SIGNOS DE INTERROGACIÓN AGREGADOS PARA QUITAR LOS WARNINGS
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}