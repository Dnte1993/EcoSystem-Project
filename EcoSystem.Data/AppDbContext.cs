using Microsoft.EntityFrameworkCore;
using EcoSystem.Data.Models;

namespace EcoSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Aquí declaramos nuestra tabla
        public DbSet<Producto> Productos { get; set; }
    }
}