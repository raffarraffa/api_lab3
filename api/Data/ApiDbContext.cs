using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<Ciudad> Ciudads { get; set; }
    public DbSet<Contrato> Contratos { get; set; }
    public DbSet<Inmueble> Inmuebles { get; set; }
    public DbSet<Inmueble_2> Inmuebles2 { get; set; }
    public DbSet<Inquilino> Inquilinos { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<TipoInmueble> TipoInmuebles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Zona> Zonas { get; set; }
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Inmueble>()
    //         .HasOne(i => i.Ciudad)
    //         .WithMany(c => c.Inmuebles)
    //         .HasForeignKey(i => i.IdCiudad);

    // }
}

