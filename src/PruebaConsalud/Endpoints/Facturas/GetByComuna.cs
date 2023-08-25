using Carter;
using Carter.OpenApi;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PruebaConsalud.DbContexts;
using PruebaConsalud.Entities;

namespace PruebaConsalud.Endpoints.Facturas;

public class GetByComuna : ICarterModule
{
    public const string route = "api/facturas/comunas";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(route, GetFacturasByComuna)
           .Produces<Dictionary<int, List<Factura>>>()
           .WithTags("Facturas");
    }

    internal static async Task<IResult> GetFacturasByComuna(ILogger<GetByComuna> logger, FacturasDbContext dbContext, int? idComuna)
    {
        var facturas = await dbContext.Facturas
            .Include(i => i.DetalleFactura)
            .ThenInclude(i => i.Producto)
            .Where(w => w.ComunaComprador == idComuna || idComuna == null)
            .ToListAsync();

        List<FacturasComuna> facturasPorComuna = new();
        foreach (var factura in facturas.GroupBy(g => g.ComunaComprador))
            facturasPorComuna.Add(new FacturasComuna(factura.Key, factura.ToList()));

        logger.LogInformation("se encontraron {Cantidad} facturas para la comuna seleccionada: {Comuna}", 
            facturasPorComuna.Count, 
            idComuna is null ? "Todas" : idComuna.ToString());

        return Results.Ok(facturasPorComuna);
    }
}

//DTO
public record FacturasComuna(float ComunaComprador, List<Factura> Facturas);
