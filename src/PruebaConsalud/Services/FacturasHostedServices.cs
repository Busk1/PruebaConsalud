using PruebaConsalud.DbContexts;
using PruebaConsalud.Entities;
using System.Text.Json;

namespace PruebaConsalud.Services;

public class FacturasHostedServices : IHostedService
{
    private readonly ILogger<FacturasHostedServices> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FacturasHostedServices(ILogger<FacturasHostedServices> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Insertando la BD");
        StreamReader file = new StreamReader(@"JsonEjemplo.json");
        var json = await file.ReadToEndAsync();
        var facturas = JsonSerializer.Deserialize<List<Factura>>(json);

        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FacturasDbContext>();

        dbContext.Facturas.AddRange(facturas!);
        dbContext.SaveChanges();

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BD Lista");
        return Task.CompletedTask;
    }
}
