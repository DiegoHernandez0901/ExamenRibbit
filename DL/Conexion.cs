using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DL
{
    public class Conexion : DbContext
    {


        public Conexion(DbContextOptions<Conexion> options) : base(options)
        {
        }
        
        public DbSet<Producto> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=mydatabase.db",
                    x => x.MigrationsAssembly("DL"));
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>().HasData(
                new Producto { Id = -1, Nombre = "Manzana", Descripcion = "Roja", Precio = 10.00m, CantidadStock = 20, FechaCreacion = new DateTime(2024, 1, 1) },
                new Producto { Id = -2, Nombre = "Cartera", Descripcion = "Cuero", Precio = 20.0m, CantidadStock = 20, FechaCreacion = new DateTime(2024, 1, 1) },
                new Producto { Id = -3, Nombre = "Girasol", Descripcion = "Amarillo", Precio = 30.00m, CantidadStock = 20, FechaCreacion = new DateTime(2024, 1, 1) }
            );
        }

        public class Producto
        {
            [Key] 
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
            public int Id { get; set; }

            [Required] 
            [StringLength(100, MinimumLength = 3)] 
            public string Nombre { get; set; }

            [StringLength(500)] 
            public string Descripcion { get; set; }

            [Required] 
            [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")] 
            public decimal Precio { get; set; }

            [Required] 
            [Range(0, int.MaxValue, ErrorMessage = "La cantidad en stock debe ser mayor o igual a 0.")] 
            public int CantidadStock { get; set; }
            
            public DateTime FechaCreacion { get; set; }
        }
    }

    public class ConexionFactory : IDesignTimeDbContextFactory<Conexion>
    {
        public Conexion CreateDbContext(string[] args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Conexion>();
            optionsBuilder.UseSqlite("Data Source=mydatabase.db");

            return new Conexion(optionsBuilder.Options);
        }
    }
}
