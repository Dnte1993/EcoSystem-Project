using System;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace EcoSystem.Client.Services
{
    public class SecureTokenService : ITokenService
    {
        private const string TokenKey = "auth_token";
        private const string ExpiryKey = "token_expiry";

        public async Task SaveTokenAsync(string token, DateTime expiration)
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
            await SecureStorage.Default.SetAsync(ExpiryKey, expiration.ToString("O"));
        }

        public async Task<string> GetTokenAsync()
        {
            var token = await SecureStorage.Default.GetAsync(TokenKey);
            var expiryStr = await SecureStorage.Default.GetAsync(ExpiryKey);

            if (token is null) return null;

            if (DateTime.TryParse(expiryStr, out var expiry) && expiry > DateTime.UtcNow)
            {
                return token;
            }

            await ClearTokenAsync();
            return null;
        }

        public Task ClearTokenAsync()
        {
            SecureStorage.Default.Remove(TokenKey);
            SecureStorage.Default.Remove(ExpiryKey);
            return Task.CompletedTask;
        }
    }
}