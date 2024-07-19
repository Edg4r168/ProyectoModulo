using Microsoft.EntityFrameworkCore;

namespace ProyectoModulo.Models;

public class VentasDbContext : DbContext
{
    public DbSet<Administrador> Administradores { get; set; }

    public DbSet<Producto> Productos { get; set; }

    public VentasDbContext(DbContextOptions options) : base(options)
    {
    }
}
