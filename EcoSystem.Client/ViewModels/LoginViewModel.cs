using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace EcoSystem.Client.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                if (_email == value) return; // Evita notificaciones redundantes
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

        public LoginViewModel()
        {
            // Comando original para el botón Entrar
            LoginCommand = new Command(EjecutarLogin);

            // Comando requerido para la Firma 3
            VerificarCommand = new Command(VerificarBinding);
        }

        private void EjecutarLogin()
        {
            Application.Current.MainPage.DisplayAlert("Validación", $"Datos recibidos para: {Email}", "OK");
        }

        private void VerificarBinding()
        {
            // Genera una cadena de asteriscos del mismo tamaño que la contraseña
            string mascaraPassword = string.IsNullOrEmpty(Password) ? "" : new string('*', Password.Length);

            // Muestra exactamente el texto que exige la rúbrica
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