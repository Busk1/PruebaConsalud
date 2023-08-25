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

public class GetByRutTests
{    
    private readonly ILogger<GetByRut> _logger = Substitute.For<ILogger<GetByRut>>();

    private DbContextOptions<FacturasDbContext> _dbContextOptions;
    public GetByRutTests()
    {
        var dbName = $"FacturaDB_Facturas_GetByComuna";
        _dbContextOptions = new DbContextOptionsBuilder<FacturasDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Fact]
    public async Task GetByRut_ShouldReturnError_WhenRutNotValid()
    {
        //Arrange
        var facturas = new FacturasTest().GetFacturas();
        var repository = await CreateRepositoryAsync(facturas);

        //Act
        var result = await GetByRut.GetFacturasByRut(_logger, repository,"12");        
        //Assert
        result.GetObjectResultValue<HttpValidationProblemDetails>()!.Status.Should().Be(400);
    }

    [Fact]
    public async Task GetByRut_ShouldReturnEmpty_WhenRutNotExist()
    {
        //Arrange
        var facturas = new FacturasTest().GetFacturas();
        var repository = await CreateRepositoryAsync(facturas);

        //Act
        var result = await GetByRut.GetFacturasByRut(_logger, repository, "19");

        //Assert
        result.GetOkObjectResultStatusCode().Should().Be(200);
        result.GetOkObjectResultValue<List<Factura>>().Should().BeEmpty();
    }

    [Fact]
    public async Task GetByRut_ShouldReturnFacturas_WhenRutExist()
    {
        //Arrange
        var facturas = new FacturasTest().GetFacturas();
        var repository = await CreateRepositoryAsync(facturas);

        //Act
        var result = await GetByRut.GetFacturasByRut(_logger, repository, "21595854k");

        //Assert
        result.GetOkObjectResultStatusCode().Should().Be(200);
        result.GetOkObjectResultValue<List<Factura>>().Should().NotBeEmpty();
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
