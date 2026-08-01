using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EcoSystem.Client.Services;
using EcoSystem.Client.Views; // ---> AGREGADO para reconocer NuevoProductoPage

namespace EcoSystem.Client.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        // ---> 1. Variables para nuestros nuevos servicios de la Firma 4
        private readonly AuthService _authService;
        private readonly ITokenService _tokenService;

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                if (_email == value) return;
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand VerificarCommand { get; }

        // ---> 2. Modificamos el constructor para inyectar las dependencias
        public LoginViewModel(AuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;

            // Actualizamos el comando para usar nuestra nueva lógica asíncrona
            LoginCommand = new Command(async () => await EjecutarLoginAsync());

            // Comando requerido para la Firma 3 (se mantiene intacto)
            VerificarCommand = new Command(async () => await VerificarBindingAsync());
        }

        // ---> 3. Lógica de autenticación contra la API en Render
        private async Task EjecutarLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Validación", "Por favor ingresa tu correo y contraseña.", "OK");
                return;
            }

            try
            {
                // Consumo de la API pasando el Email como usuario
                var result = await _authService.LoginAsync(Email, Password);

                if (result.Success)
                {
                    // 1. Guardamos el token de forma segura
                    await _tokenService.SaveTokenAsync(result.Token, DateTime.UtcNow.AddHours(1));

                    // 2. Mostramos la alerta definitiva de éxito
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Inicio de sesión correcto.", "OK");

                    // 3. ---> NUEVO: Navegamos a la pantalla de crear producto <---
                    await Shell.Current.GoToAsync(nameof(NuevoProductoPage));
                }
                else
                {
                    // Muestra los errores 401, 403, etc.
                    await Application.Current.MainPage.DisplayAlert("Error de acceso", result.ErrorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error inesperado", ex.Message, "OK");
            }
        }

        private async Task VerificarBindingAsync()
        {
            string mascaraPassword = string.IsNullOrEmpty(Password) ? "" : new string('*', Password.Length);

            // Recuperamos el token usando tu interfaz inyectada
            string tokenGuardado = await _tokenService.GetTokenAsync();

            string mensaje = $"Usuario en ViewModel: {Email}\n" +
                             $"Contraseña en ViewModel: {mascaraPassword}\n\n" +
                             $"Token Guardado:\n{(string.IsNullOrEmpty(tokenGuardado) ? "Ningún token guardado" : tokenGuardado)}";

            await Application.Current.MainPage.DisplayAlert("Verificación de Seguridad", mensaje, "OK");
        }

        // --- Implementación de INotifyPropertyChanged ---
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}