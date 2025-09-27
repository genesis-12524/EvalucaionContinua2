using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Models;

namespace PortalInmobiliario.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<Inmueble>(entity =>
            {
                entity.HasIndex(i => i.Codigo).IsUnique();
                
                entity.Property(i => i.Precio).HasColumnType("decimal(18,2)");
                
                
                entity.HasCheckConstraint("CK_Inmueble_Precio_Positivo", "Precio > 0");
                
                
                entity.HasCheckConstraint("CK_Inmueble_Metros_Positivo", "MetrosCuadrados > 0");
            });

            builder.Entity<Visita>(entity =>
            {
                
                entity.HasCheckConstraint("CK_Visita_Fechas_Validas", "FechaInicio < FechaFin");
                
                
                entity.HasIndex(v => new { v.InmuebleId, v.FechaInicio, v.FechaFin });
            });

            builder.Entity<Reserva>(entity =>
            {
                
                entity.HasIndex(r => new { r.InmuebleId, r.FechaExpiracion });
            });

            
            builder.Entity<Inmueble>().HasData(
                new Inmueble 
                { 
                    Id = 1, 
                    Codigo = "DEP-001", 
                    Titulo = "Departamento moderno en Miraflores", 
                    Imagen = "/img/dep1.jpg",
                    Tipo = TipoInmueble.Departamento, 
                    Ciudad = "Lima", 
                    Direccion = "Av. Larco 123", 
                    Dormitorios = 2, 
                    Banos = 2, 
                    MetrosCuadrados = 80, 
                    Precio = 150000, 
                    Activo = true 
                },
                new Inmueble 
                { 
                    Id = 2, 
                    Codigo = "CASA-001", 
                    Titulo = "Casa familiar en La Molina", 
                    Imagen = "/img/casa1.jpg",
                    Tipo = TipoInmueble.Casa, 
                    Ciudad = "Lima", 
                    Direccion = "Calle Los Olivos 456", 
                    Dormitorios = 4, 
                    Banos = 3, 
                    MetrosCuadrados = 200, 
                    Precio = 450000, 
                    Activo = true 
                },
                new Inmueble 
                { 
                    Id = 3, 
                    Codigo = "OFI-001", 
                    Titulo = "Oficina en centro financiero", 
                    Imagen = "/img/oficina1.jpg",
                    Tipo = TipoInmueble.Oficina, 
                    Ciudad = "Lima", 
                    Direccion = "Av. Javier Prado 789", 
                    Dormitorios = 0, 
                    Banos = 2, 
                    MetrosCuadrados = 120, 
                    Precio = 200000, 
                    Activo = true 
                },
                new Inmueble 
                { 
                    Id = 4, 
                    Codigo = "LOC-001", 
                    Titulo = "Local comercial en zona alta", 
                    Imagen = "/img/local1.jpg",
                    Tipo = TipoInmueble.Local, 
                    Ciudad = "Arequipa", 
                    Direccion = "Calle Mercaderes 321", 
                    Dormitorios = 0, 
                    Banos = 1, 
                    MetrosCuadrados = 150, 
                    Precio = 180000, 
                    Activo = true 
                }
            );
        }
    



}
