using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PruebaConsalud.DbContexts;
using PruebaConsalud.Endpoints.Compradores;
using PruebaConsalud.Entities;

namespace PruebaConsalud.Endpoints.Facturas;

public class GetAll : ICarterModule
{
    public const string route = "api/facturas";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(route, GetAllFacturas)
           .Produces<IEnumerable<Factura>>()
           .WithTags("Facturas");
    }
    internal static async Task<IResult> GetAllFacturas(ILogger<GetAll> logger, FacturasDbContext dbContext)
    {
        var facturas = await dbContext.Facturas
            .Include(i => i.DetalleFactura)
            .ThenInclude(i => i.Producto)
        .ToListAsync();

        logger.LogInformation("se encontraron {Cantidad} facturas", facturas.Count);

        foreach (var factura in facturas)
            factura.TotalFactura = factura.DetalleFactura.Sum(c => c.TotalProducto);

        return Results.Ok(facturas);
    }
}