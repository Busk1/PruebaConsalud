using Serilog;

namespace Microsoft.Extensions.Hosting;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddConfiguration(this IHostBuilder host) =>
        host.ConfigureAppConfiguration((context, config) =>
        {
            const string configurationsDirectory = "Configurations";
            var env = context.HostingEnvironment;
            var settings = new string[] { "logger", "authentication" };

            //initial config
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables();

            //custom config
            foreach (var setting in settings)
                config.AddJsonFile($"{configurationsDirectory}/{setting}.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"{configurationsDirectory}/{setting}.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        });

    public static IHostBuilder AddSerilog(this IHostBuilder host, IConfiguration configuration) =>
        host.UseSerilog((_, config) => config.ReadFrom.Configuration(configuration));
}
