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

public class GetAllTests
{    
    private readonly ILogger<GetAll> _logger = Substitute.For<ILogger<GetAll>>();

    private DbContextOptions<FacturasDbContext> _dbContextOptions;
    public GetAllTests()
    {
        var dbName = $"FacturaDB_Facturas_GetAll";
        _dbContextOptions = new DbContextOptionsBuilder<FacturasDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmpty_WhenNoFacturasInDB()
    {
        //Arrange
        var empty = Enumerable.Empty<Factura>();
        var repository = await CreateRepositoryAsync(empty);

        //Act
        var result = await GetAll.GetAllFacturas(_logger, repository);

        //Assert
        result.GetOkObjectResultStatusCode().Should().Be(200);
        result.GetOkObjectResultValue<List<Factura>>().Should().BeEquivalentTo(empty);
    }

    [Fact]
    public async Task GetAll_ShouldReturnFacturas_WhenFacturasInDB()
    {
        //Arrange
        var facturas = new FacturasTest().GetFacturas();
        var repository = await CreateRepositoryAsync(facturas);

        //Act
        var result = await GetAll.GetAllFacturas(_logger, repository);

        //Assert
        result.GetOkObjectResultStatusCode().Should().Be(200);
        result.GetOkObjectResultValue<List<Factura>>().Should().BeEquivalentTo(facturas);
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
