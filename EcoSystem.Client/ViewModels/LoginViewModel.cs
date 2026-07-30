using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EcoSystem.Client.Services;

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
            VerificarCommand = new Command(VerificarBinding);
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
                    // Si responde un 200 OK, guardamos el token de forma segura
                    await _tokenService.SaveTokenAsync(result.Token, DateTime.UtcNow.AddHours(1));

                    await Application.Current.MainPage.DisplayAlert("Éxito", "Inicio de sesión correcto.", "OK");

                    // Aquí posteriormente pondrás la navegación a tu página principal:
                    // await Shell.Current.GoToAsync("//MainPage");
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

        private void VerificarBinding()
        {
            string mascaraPassword = string.IsNullOrEmpty(Password) ? "" : new string('*', Password.Length);

            Application.Current.MainPage.DisplayAlert(
                "Verificación de Binding",
                $"Usuario en ViewModel: {Email}\nContraseña en ViewModel: {mascaraPassword}",
                "OK");
        }

        // --- Implementación de INotifyPropertyChanged ---
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}