using Carter;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using PruebaConsalud.DbContexts;
using PruebaConsalud.Extensions;
using PruebaConsalud.Services;
using Reguto.DI.Microsoft;
using Reguto.Options.Microsoft;
using Serilog;

var builder = WebApplication.CreateBuilder();

var configuration = builder.Configuration;
var env = builder.Environment;

builder.Host
    .AddConfiguration()
    .AddSerilog(configuration);

builder.Services
    .AddHttpContextAccessor()
    .AddSwagger(configuration)
    .AddCarter()
    .AddDbContext<FacturasDbContext>(options => options.UseInMemoryDatabase("FacturasDb"))
    .Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal)
    .AddResponseCompression(configureOptions => configureOptions.EnableForHttps = true)
    .AddHostedService<FacturasHostedServices>()
    .AddHealthChecks();

builder.Services.AddReguto();
builder.Services.ConfigureReguto(configuration);

var app = builder.Build();


app.UseHttpsRedirection();
app.MapSwagger();
app.UseSwaggerUI();
app.MapCarter();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseExceptionHandler(err => err.UseCustomErrors(builder.Environment.IsDevelopment()));
app.UseHealthChecks("/health");
app.UseSerilogRequestLogging();

app.Run();