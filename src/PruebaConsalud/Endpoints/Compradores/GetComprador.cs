using Carter;
using Microsoft.EntityFrameworkCore;
using PruebaConsalud.DbContexts;

namespace PruebaConsalud.Endpoints.Compradores;

public class GetComprador : ICarterModule
{
    public const string route = "api/compradores/topcompras";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(route, GetTopComprador)
           .Produces<TopComprador>()
           .WithTags("Compradores");
    }
    internal static async Task<IResult> GetTopComprador(ILogger<GetComprador> logger, FacturasDbContext dbContext)
    {
        var topComprador = await dbContext.Facturas
            .GroupBy(g => g.RUTComprador)
            .Select(s => new { RutComprador = s.Key, Count = s.Count() })
            .OrderByDescending(o => o.Count)
            .FirstOrDefaultAsync();

        if (topComprador is null)
            return Results.Ok();

        logger.LogInformation("el top comprador fue {@TopComprador}", topComprador);

        return Results.Ok(new TopComprador(topComprador!.RutComprador, topComprador.Count));
    }
}

//DTO
public record TopComprador(float RUTComprador, int TotalCompras);