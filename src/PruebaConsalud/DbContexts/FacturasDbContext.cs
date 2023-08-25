using Microsoft.EntityFrameworkCore;
using PruebaConsalud.Entities;

namespace PruebaConsalud.DbContexts;

public class FacturasDbContext: DbContext
{
    public FacturasDbContext(DbContextOptions<FacturasDbContext> options): base(options)
    {}

    public DbSet<Factura> Facturas { get; set; }
    public DbSet<Detallefactura> DetalleFacturas { get; set; }
    public DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Factura>(a =>
        {
            a.HasKey(nameof(Factura.NumeroDocumento));
            a.HasMany<Detallefactura>();
        });

        modelBuilder.Entity<Detallefactura>(a =>
        {
            a.HasOne<Producto>();
        });
    }
}
