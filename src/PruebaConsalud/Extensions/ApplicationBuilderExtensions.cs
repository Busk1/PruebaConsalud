namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MapSwagger(this IApplicationBuilder app) =>
        app.UseSwagger()
        .UseSwaggerUI();
}
