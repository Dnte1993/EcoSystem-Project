using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
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
                await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Validación", "Por favor ingresa tu correo y contraseña.", "OK");
                return;
            }

            try
            {
                var result = await _authService.LoginAsync(Email, Password);

                if (result.Success)
                {
                    await SecureStorage.Default.SetAsync("jwt_token", result.Token);
                    var expDate = DateTime.UtcNow.AddHours(1);
                    await SecureStorage.Default.SetAsync("jwt_exp", expDate.ToString("O"));

                    await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Éxito", "Inicio de sesión correcto.", "OK");
                    await Shell.Current.GoToAsync("//ListaProductosPage");
                }
                else
                {
                    await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Error de acceso", result.ErrorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Error inesperado", ex.Message, "OK");
            }
        }

        private async Task VerificarBindingAsync()
        {
            string mascaraPassword = string.IsNullOrEmpty(Password) ? "" : new string('*', Password.Length);
            string tokenGuardado = await SecureStorage.Default.GetAsync("jwt_token") ?? string.Empty;

            string mensaje = $"Usuario en ViewModel: {Email}\n" +
                             $"Contraseña en ViewModel: {mascaraPassword}\n\n" +
                             $"Token Guardado:\n{(string.IsNullOrEmpty(tokenGuardado) ? "Ningún token guardado" : tokenGuardado)}";

            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Verificación de Seguridad", mensaje, "OK");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}