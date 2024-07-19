using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoModulo.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        public int Stock { get; set; }

        public string? UrlImagen { get; set; }

        public string? NombreImagen { get; set; }

        [Required]
        public decimal Precio { get; set; }
    }
}
