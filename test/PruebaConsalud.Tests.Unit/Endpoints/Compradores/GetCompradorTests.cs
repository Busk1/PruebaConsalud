using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PruebaConsalud.DbContexts;
using PruebaConsalud.Endpoints.Compradores;
using PruebaConsalud.Entities;
using PruebaConsalud.Tests.Unit.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PruebaConsalud.Tests.Unit.Endpoints.Compradores;

public class GetCompradorTests
{    
    private readonly ILogger<GetComprador> _logger = Substitute.For<ILogger<GetComprador>>();

    private DbContextOptions<FacturasDbContext> _dbContextOptions;
    public GetCompradorTests()
    {
        var dbName = $"FacturaDB_Compradores_GetComprador";
        _dbContextOptions = new DbContextOptionsBuilder<FacturasDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Fact]
    public async Task GetComprador_ShouldReturnNull_WhenNoFacturasInDB()
    {
        //Arrange
        var empty = Enumerable.Empty<Factura>();
        var repository = await CreateRepositoryAsync(empty);

        //Act
        var result = await GetComprador.GetTopComprador(_logger, repository);

        //Assert
        result.GetOkObjectResultStatusCode().Should().Be(200);
        result.GetOkObjectResultValue<TopComprador>().Should().BeNull();
    }

    [Fact]
    public async Task GetComprador_ShouldReturnTopComprador_WhenFacturasInDB()
    {
        //Arrange
        var facturas = new FacturasTest().GetFacturas();
        var repository = await CreateRepositoryAsync(facturas);

        //Act
        var result = await GetComprador.GetTopComprador(_logger, repository);

        //Assert
        result.GetOkObjectResultStatusCode().Should().Be(200);
        result.GetOkObjectResultValue<TopComprador>().Should().NotBeNull();
    }

    private async Task<FacturasDbContext> CreateRepositoryAsync(IEnumerable<Factura> facturas)
    {
        var context = new FacturasDbContext(_dbContextOptions);
        await PopulateDataAsync(context, facturas);
        return context;
    }

    private async Task PopulateDataAsync(FacturasDbContext context, IEnumerable<Factura> facturas)
    {
        //clear the database}
        context!.Facturas.RemoveRange(context.Facturas.ToList());
        await context.SaveChangesAsync();

        //set the new data
        await context!.Facturas.AddRangeAsync(facturas);
        await context.SaveChangesAsync();
    }
}
