using CBS.Core;
using CBS.Core.Accounts.Services;
using CBS.Core.Clients.Services;
using CBS.Core.Infrastructure.Providers.Rates;
using CBS.Infrastructure.Data.Accounts;
using CBS.Infrastructure.Data.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CBS.Infrastructure.Data;

public static class DependencyInjection
{
  public static IServiceCollection AddData(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<IClientRepository, ClientRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IConvertRateProvider, InMemoryConvertRateProvider>();

    services.AddDbContext<CbsContext>(options => options
      .UseSqlite(config.GetConnectionString("DefaultConnection")));

    return services;
  }
}
