using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // Agregado para usar SecureStorage de forma directa
using EcoSystem.Client.Services;
using EcoSystem.Client.Views;

namespace EcoSystem.Client.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;

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

        // Retiramos ITokenService para evitar conflictos de llaves y usar SecureStorage directo
        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await EjecutarLoginAsync());
            VerificarCommand = new Command(async () => await VerificarBindingAsync());
        }

        private async Task EjecutarLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Validación", "Por favor ingresa tu correo y contraseña.", "OK");
                return;
            }

            try
            {
                var result = await _authService.LoginAsync(Email, Password);

                if (result.Success)
                {
                    // 1. Guardamos el token con la llave EXACTA que busca el AuthHandler
                    await SecureStorage.Default.SetAsync("jwt_token", result.Token);

                    // 2. Guardamos la expiración en formato Universal (UTC) "Roundtrip" para evitar bugs de zona horaria
                    var expDate = DateTime.UtcNow.AddHours(1);
                    await SecureStorage.Default.SetAsync("jwt_exp", expDate.ToString("O"));

                    await Application.Current.MainPage.DisplayAlert("Éxito", "Inicio de sesión correcto.", "OK");

                    // 3. CORRECCIÓN: Navegamos a la raíz del inventario
                    await Shell.Current.GoToAsync("//ListaProductosPage");
                }
                else
                {
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

            // Leemos la llave exacta para confirmar que sí se guardó
            string tokenGuardado = await SecureStorage.Default.GetAsync("jwt_token");

            string mensaje = $"Usuario en ViewModel: {Email}\n" +
                             $"Contraseña en ViewModel: {mascaraPassword}\n\n" +
                             $"Token Guardado:\n{(string.IsNullOrEmpty(tokenGuardado) ? "Ningún token guardado" : tokenGuardado)}";

            await Application.Current.MainPage.DisplayAlert("Verificación de Seguridad", mensaje, "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}