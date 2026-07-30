using System;
using System.Threading.Tasks;

namespace EcoSystem.Client.Services
{
    public interface ITokenService
    {
        Task SaveTokenAsync(string token, DateTime expiration);
        Task<string> GetTokenAsync();
        Task ClearTokenAsync();
    }
}