using CBS.Api.Middlewares;
using CBS.Core;
using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Services;
using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;
using CBS.Core.Infrastructure.Data;
using CBS.Core.Infrastructure.Data.Accounts;
using CBS.Core.Infrastructure.Data.Clients;
using CBS.Core.Infrastructure.Providers.Rates;
using CBS.Core.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Init DI for Core
builder.Services.AddSingleton<ITable<Account>, Table<Account>>();
builder.Services.AddSingleton<ITable<Client>, Table<Client>>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UnitOfWork>());
builder.Services.AddScoped<IChangeTracker>(sp => sp.GetRequiredService<UnitOfWork>());
builder.Services.AddScoped<IConvertRateProvider, InMemoryConvertRateProvider>();
builder.Services.AddScoped<IAccountRepository, InMemoryAccountRepository>();
builder.Services.AddScoped<IClientRepository, InMemoryClientRepository>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<CreateClientUseCase>();
builder.Services.AddScoped<GetClientByIdUseCase>();
builder.Services.AddScoped<CreateAccountUseCase>();
builder.Services.AddScoped<TransferUseCase>();

// Create and run app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
