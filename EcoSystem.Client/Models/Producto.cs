using System.Text.Json.Serialization;

namespace EcoSystem.Client.Models
{
    public class Producto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("stock")]
        public int Stock { get; set; }
    }
}