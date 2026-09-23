using Shreksoft.Bank.Core.Domain.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shreksoft.Bank.Infrastructure.Data.Transfers;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.HasKey(t => t.Id);
        builder.ComplexProperty(t => t.SenderSide, ConfigureSide);
        builder.ComplexProperty(t => t.RecipientSide, ConfigureSide);
    }

    private static void ConfigureSide(ComplexPropertyBuilder<TransferSide> side)
    {
        side.Property(s => s.AccountId);
        side.Property(s => s.ClientId);
        side.ComplexProperty(s => s.Currency, curr =>
        {
            curr.Property(c => c.Code).HasConversion<string>();
            curr.Property(c => c.Scale);
        });
    }
}
