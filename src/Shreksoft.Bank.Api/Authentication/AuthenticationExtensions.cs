namespace Shreksoft.Bank.Api.Authentication;

public enum AuthMode
{
    Jwt
}

public class AuthSettings
{
    public const string SectionName = "Auth";

    public AuthMode Mode { get; set; } = AuthMode.Jwt;
    public string? Audience { get; set; }
    public string? AuthorityUri { get; set; }
}

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        var settings = config
            .GetSection(AuthSettings.SectionName)
            .Get<AuthSettings>() ?? throw new InvalidOperationException("Auth section is missing");

        switch (settings.Mode)
        {
            case AuthMode.Jwt: Jwt.AddSupport(services, settings); break;
            default: throw new NotSupportedException("Unknown auth mode");
        }

        services.AddAuthorization();

        return services;
    }
}
