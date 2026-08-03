using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // <-- Necesario para ObservableCollection
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

        private Producto? _productoEnEdicion;

        // NUEVO: Lista reactiva para la interfaz
        public ObservableCollection<Producto> ProductosRecientes { get; } = new ObservableCollection<Producto>();

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
                await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Validación", "Ingresa un nombre válido, un precio mayor a 0 y un stock válido.", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                bool exito = false;
                var productoCreado = new Producto(); // Para guardarlo temporalmente en memoria

                if (_productoEnEdicion == null)
                {
                    productoCreado = new Producto
                    {
                        Nombre = Nombre,
                        Precio = Precio,
                        Stock = Stock
                    };

                    exito = await _apiService.CrearProductoAsync(productoCreado);
                }
                else
                {
                    _productoEnEdicion.Nombre = Nombre;
                    _productoEnEdicion.Precio = Precio;
                    _productoEnEdicion.Stock = Stock;

                    exito = await _apiService.ActualizarProductoAsync(_productoEnEdicion.Id, _productoEnEdicion);
                }

                if (exito)
                {
                    if (_productoEnEdicion == null)
                    {
                        // ES UN PRODUCTO NUEVO
                        // 1. Lo agregamos al inicio de la lista visual (índice 0)
                        ProductosRecientes.Insert(0, productoCreado);

                        // 2. Limpiamos los campos para el siguiente producto
                        Nombre = string.Empty;
                        Precio = 0;
                        Stock = 0;

                        await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Éxito", "Producto agregado a la base de datos.", "OK");
                    }
                    else
                    {
                        // ES UNA EDICIÓN
                        await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Éxito", "Producto actualizado correctamente.", "OK");
                        _productoEnEdicion = null;
                        await Shell.Current.GoToAsync(".."); // Solo regresamos si estábamos editando
                    }
                }
                else
                {
                    await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Error", "No se pudo guardar. Verifica tu conexión.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Error inesperado", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}