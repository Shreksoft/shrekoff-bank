using Shreksoft.Bank.Api.Middlewares;
using Shreksoft.Bank.Application.Accounts;
using Shreksoft.Bank.Application.Clients;
using Shreksoft.Bank.Infrastructure.Data;
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

// Create and run app
var app = builder.Build();

// Migration process
using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<BankDbContext>().Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
