using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoModulo.Models;

public class Administrador
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [NotMapped]
    public bool KeepLoggedIn { get; set; }
}
