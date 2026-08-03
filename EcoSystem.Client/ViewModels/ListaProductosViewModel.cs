using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EcoSystem.Client.Models;
using EcoSystem.Client.Services;
using System.Collections.Generic;

namespace EcoSystem.Client.ViewModels
{
    public class ListaProductosViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

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
        public ICommand EditarCommand { get; }
        public ICommand EliminarCommand { get; }

        public ListaProductosViewModel(ApiService apiService)
        {
            _apiService = apiService;
            CargarProductosCommand = new Command(async () => await CargarProductosAsync());
            EditarCommand = new Command<Producto>(OnEditar);
            EliminarCommand = new Command<Producto>(OnEliminar);
        }

        public async Task CargarProductosAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Productos.Clear();
                var productosDesdeApi = await _apiService.GetProductosAsync();

                foreach (var prod in productosDesdeApi)
                {
                    Productos.Add(prod);
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Error", $"No se pudieron cargar los productos: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnEditar(Producto producto)
        {
            if (producto == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "ProductoSeleccionado", producto }
            };

            await Shell.Current.GoToAsync("NuevoProductoPage", navigationParameter);
        }

        private async void OnEliminar(Producto producto)
        {
            if (producto == null) return;

            bool respuesta = await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
                "Confirmar Eliminación",
                $"¿Estás seguro de que deseas eliminar '{producto.Nombre}'?",
                "Sí, eliminar",
                "Cancelar");

            if (!respuesta) return;
            IsBusy = true;

            try
            {
                bool exito = await _apiService.EliminarProductoAsync(producto.Id);

                if (exito)
                {
                    Productos.Remove(producto);
                }
                else
                {
                    await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Error", "No se pudo eliminar el producto en la base de datos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Error", $"Ocurrió un problema: {ex.Message}", "OK");
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