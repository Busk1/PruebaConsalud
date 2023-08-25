using Reguto.Options.Abstractions;

namespace PruebaConsalud.Settings;

[Options("Authentication")]
public class AuthenticationSettings
{
    public readonly string SectionName = "Authentication";
    public string ApiKey { get; set; }
    public string Header { get; set; }
}
