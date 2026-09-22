using CBS.Application;
using CBS.Application.Accounts;
using CBS.Application.Clients;
using CBS.Infrastructure.Data.Accounts;
using CBS.Infrastructure.Data.Clients;
using CBS.Infrastructure.Data.Providers.Rates;
using CBS.Infrastructure.Data.Transfers;
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
    services.AddScoped<ITransferRepository, TransferRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IConvertRateProvider, InMemoryConvertRateProvider>();

    services.AddDbContext<CbsContext>(options => options
      .UseSqlite(config.GetConnectionString("DefaultConnection")));

    return services;
  }
}
