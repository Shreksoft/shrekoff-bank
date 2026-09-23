using Shreksoft.Bank.Application;
using Shreksoft.Bank.Application.Accounts;
using Shreksoft.Bank.Application.Clients;
using Shreksoft.Bank.Infrastructure.Data.Accounts;
using Shreksoft.Bank.Infrastructure.Data.Clients;
using Shreksoft.Bank.Infrastructure.Data.Providers.Rates;
using Shreksoft.Bank.Infrastructure.Data.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shreksoft.Bank.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ITransferRepository, TransferRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IConvertRateProvider, InMemoryConvertRateProvider>();

        services.AddDbContext<BankDbContext>(options => options
            .UseSqlite(config.GetConnectionString("DefaultConnection")));

        return services;
    }
}
