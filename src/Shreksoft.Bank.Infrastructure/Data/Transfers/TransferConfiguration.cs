using Shreksoft.Bank.Core.Domain.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shreksoft.Bank.Infrastructure.Data.Accounts;

namespace Shreksoft.Bank.Infrastructure.Data.Transfers;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.HasKey(t => t.Id);
        builder.ComplexProperty(t => t.SenderSide, ConfigureSide);
        builder.ComplexProperty(t => t.RecipientSide, ConfigureSide);
        builder.ComplexProperty(t => t.CommissionMoney, MoneyConfiguration.Configure);
        builder.ComplexProperty(t => t.RecipientMoney, MoneyConfiguration.Configure);
        builder.ComplexProperty(t => t.SenderMoney, MoneyConfiguration.Configure);
    }

    private static void ConfigureSide(ComplexPropertyBuilder<TransferSide> side)
    {
        side.Property(s => s.AccountId);
        side.Property(s => s.ClientId);
    }
}
