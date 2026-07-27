using CBS.Core.Accounts.Infrastructure;
using CBS.Core.Accounts.Services;
using CBS.Core.Clients.Infrastructure;
using CBS.Core.Clients.Services;
using CBS.Core.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Init DI for Core
builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
builder.Services.AddSingleton<IClientRepository, InMemoryClientRepository>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<ClientService>();
builder.Services.AddSingleton<CreateClientUseCase>();
builder.Services.AddSingleton<GetClientByIdUseCase>();

// Create and run app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
