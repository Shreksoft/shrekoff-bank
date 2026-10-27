using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Shreksoft.Bank.Api.Authentication;

public static class Jwt
{
    public static void AddSupport(IServiceCollection services, AuthSettings s)
    {
        if (s.Audience == null)
            throw new InvalidOperationException("Audience field is missing");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                option.Audience = s.Audience;
                option.Authority = s.AuthorityUri;
                option.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(30);

                option.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("FAILED: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                };
            });
    }
}
