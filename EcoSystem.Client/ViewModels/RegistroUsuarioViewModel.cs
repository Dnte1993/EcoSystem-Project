using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using EcoSystem.Client.Models; // Asegúrate de que coincida con el namespace de tu DTO

namespace EcoSystem.Client.ViewModels
{
    public class RegistroUsuarioViewModel : BindableObject
    {
        private string _username;
        private string _password;
        private bool _isBusy;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegistrarCommand { get; }

        public RegistroUsuarioViewModel()
        {
            RegistrarCommand = new Command(async () => await RegistrarUsuarioAsync());
        }

        private async Task RegistrarUsuarioAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor ingresa usuario y contraseña.", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                // NOTA: Cambia "localhost:7123" por el puerto real donde corre tu API
                string apiUrl = "http://localhost:5124/api/Auth/register";

                var newUser = new UserLoginDto
                {
                    Username = this.Username,
                    Password = this.Password
                };

                using var client = new HttpClient();
                var response = await client.PostAsJsonAsync(apiUrl, newUser);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");
                    // Regresamos al Login
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    string errorMsg = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo registrar: {errorMsg}", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Problema de conexión: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}