using System.ComponentModel.DataAnnotations;

namespace EcoSystem.Data.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public decimal Precio { get; set; }

        public int Stock { get; set; }
    }
}