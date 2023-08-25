using Microsoft.Extensions.Options;
using PruebaConsalud.Settings;

namespace PruebaConsalud.Services;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationMiddleware> _logger;
    private readonly AuthenticationSettings _authenticationSettings;

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger, IOptions<AuthenticationSettings> authenticationSettings)
    {
        _next = next;
        _logger = logger;
        _authenticationSettings = authenticationSettings.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        if(!context.Request.Headers.TryGetValue(_authenticationSettings.Header, out var key))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync($"Debes incluir la ApiKey en el encabezado (encabezado: {_authenticationSettings.Header})");
            _logger.LogWarning("No se incluyo la ApiKey en el encabezado (encabezado: {Encabezado})", _authenticationSettings.Header);
            return;
        }

        if(!_authenticationSettings.ApiKey.Equals(key))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync($"La ApiKey proporcionada no es válida");
            _logger.LogWarning("se ingresó una ApiKey inválida, (ApiKey: {ApiKey})", key);
            return;
        }
        await _next(context);

    }
}
