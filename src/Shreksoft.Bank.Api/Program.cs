using Shreksoft.Bank.Api.Middlewares;
using Shreksoft.Bank.Application.Accounts;
using Shreksoft.Bank.Application.Clients;
using Shreksoft.Bank.Infrastructure.Data;
using Shreksoft.Bank.Api.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Init DI for Core
builder.Services
    .AddData(builder.Configuration)
    .AddAuth(builder.Configuration)
    .AddScoped<AccountService>()
    .AddScoped<ClientService>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        var tokenUri = builder.Configuration
            .GetSection(AuthSettings.SectionName).Get<AuthSettings>()?.TokenUri;

        if (string.IsNullOrEmpty(tokenUri))
        {
            Console.WriteLine("Warn: TokenUri is missing");
            return Task.CompletedTask;
        }

        document.Info = new OpenApiInfo { Title = "ShrekoffBank.Api", Version = "v1" };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["oauth2"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(tokenUri),
                        Scopes = new Dictionary<string, string> { ["api"] = "nothing now, bcs demo returns 'api' scope by default" }
                    }
                }
            }
        };

        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("oauth2", document)] = ["api"]
            }
        ];

        return Task.CompletedTask;
    });
});

// Create and run app
var app = builder.Build();

// Migration process
using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<BankDbContext>().Database.MigrateAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "ShrekoffBank.Api v1")
    );
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
