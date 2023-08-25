using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PruebaConsalud.DbContexts;
using PruebaConsalud.Entities;
using PruebaConsalud.Validators;

namespace PruebaConsalud.Endpoints.Facturas;

public class GetByRut : ICarterModule
{
    public const string route = "api/facturas/{rutComprador}";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(route, GetFacturasByRut)
           .Produces<IEnumerable<Factura>>()
           .WithTags("Facturas");
    }

    internal static async Task<IResult> GetFacturasByRut(ILogger<GetByRut> logger, FacturasDbContext dbContext, string rutComprador)
    {
        var rutIsValid = RutValidator.MustBeValid(rutComprador);
        if (!rutIsValid)
        {
            logger.LogInformation("el rut ingresado no es valido: {Rut}", rutComprador);
            return Results.ValidationProblem(statusCode: 400, errors: new Dictionary<string, string[]>() {
                { "RutComprador", new string[] {"El rut debe ser valido" } }
            });
        }

        var facturas =
            await dbContext.Facturas
            .Include(i => i.DetalleFactura)
            .ThenInclude(i => i.Producto)
            .Where(w => w.RUTComprador == int.Parse(rutComprador.Substring(0, rutComprador.Length - 1)))
            .ToListAsync();

        logger.LogInformation("se encontraron {Cantidad} facturas con el rut: {Rut}", facturas.Count, rutComprador);

        return Results.Ok(facturas);
    }
}
