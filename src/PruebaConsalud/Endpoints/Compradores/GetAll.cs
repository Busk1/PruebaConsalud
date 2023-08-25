using Carter;
using Microsoft.EntityFrameworkCore;
using PruebaConsalud.DbContexts;

namespace PruebaConsalud.Endpoints.Compradores;

public class GetAll : ICarterModule
{
    public const string route = "api/compradores";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(route, GetAllCompradores)
           .Produces<Comprador>()
           .WithTags("Compradores");
    }
    internal static async Task<IResult> GetAllCompradores(ILogger<GetAll> logger, FacturasDbContext dbContext)
    {
        var facturas = await dbContext.Facturas
            .Include(i => i.DetalleFactura)
            .ToListAsync();

        HashSet<Comprador> compradores = new();
        foreach (var facturasPorComprador in facturas.GroupBy(g => g.RUTComprador))
        {
            var totalProducto = facturasPorComprador.Sum(s => s.DetalleFactura.Sum(ss => ss.TotalProducto));
            compradores.Add(new Comprador(facturasPorComprador.Key, totalProducto));            
        }

        logger.LogInformation("Se encontraron {Cantidad} compradores", compradores.Count);

        return Results.Ok(compradores);
    }
}

//DTO
public record Comprador(float RUTComprador, float MontoCompras);