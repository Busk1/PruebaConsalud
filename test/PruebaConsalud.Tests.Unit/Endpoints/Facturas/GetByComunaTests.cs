using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PruebaConsalud.DbContexts;
using PruebaConsalud.Endpoints.Facturas;
using PruebaConsalud.Entities;
using PruebaConsalud.Tests.Unit.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PruebaConsalud.Tests.Unit.Endpoints.Facturas;

public class GetByComunaTests
{    
    private readonly ILogger<GetByComuna> _logger = Substitute.For<ILogger<GetByComuna>>();

    private DbContextOptions<FacturasDbContext> _dbContextOptions;
    public GetByComunaTests()
    {
        var dbName = $"FacturaDB_Facturas_GetByRut";
        _dbContextOptions = new DbContextOptionsBuilder<FacturasDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Fact]
    public async Task GetByComuna_ShouldReturnFacturas_WhenComunaCompradorNotSent()
    {
        //Arrange
        var facturas = new FacturasTest().GetFacturas();
        var repository = await CreateRepositoryAsync(facturas);

        //Act
        var result = await GetByComuna.GetFacturasByComuna(_logger, repository,null);

        //Assert
        result.GetOkObjectResultStatusCode().Should().Be(200);
        result.GetOkObjectResultValue<List<FacturasComuna>>().Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetByComuna_ShouldReturnEmpty_WhenComunaCompradorSentAndNotExist()
    {
        //Arrange
        var facturas = new FacturasTest().GetFacturas();
        var repository = await CreateRepositoryAsync(facturas);

        //Act
        var result = await GetByComuna.GetFacturasByComuna(_logger, repository, 1);

        //Assert
        result.GetOkObjectResultStatusCode().Should().Be(200);
        result.GetOkObjectResultValue<List<FacturasComuna>>().Should().BeEmpty();
    }

    [Fact]
    public async Task GetByComuna_ShouldReturnFacturas_WhenComunaCompradorSentAndExist()
    {
        //Arrange
        var facturas = new FacturasTest().GetFacturas();
        var repository = await CreateRepositoryAsync(facturas);

        //Act
        var result = await GetByComuna.GetFacturasByComuna(_logger, repository, 65);

        //Assert
        result.GetOkObjectResultStatusCode().Should().Be(200);
        result.GetOkObjectResultValue<List<FacturasComuna>>().Should().NotBeEmpty();
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
