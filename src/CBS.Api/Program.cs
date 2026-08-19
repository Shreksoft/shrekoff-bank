using CBS.Api.Middlewares;
using CBS.Core.Accounts.Services;
using CBS.Core.Clients.Services;
using CBS.Core.UseCases;
using CBS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Init DI for Core
builder.Services.AddData(builder.Configuration);
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<CreateClientUseCase>();
builder.Services.AddScoped<GetClientByIdUseCase>();
builder.Services.AddScoped<CreateAccountUseCase>();
builder.Services.AddScoped<TransferUseCase>();

// Create and run app
var app = builder.Build();

// Migration process
using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<CbsContext>().Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
