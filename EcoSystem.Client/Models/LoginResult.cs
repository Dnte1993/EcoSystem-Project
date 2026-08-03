namespace EcoSystem.Client.Models
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}