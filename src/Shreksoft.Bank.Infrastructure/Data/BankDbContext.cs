using Shreksoft.Bank.Core.Domain.Accounts;
using Shreksoft.Bank.Core.Domain.Clients;
using Shreksoft.Bank.Core.Domain.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Shreksoft.Bank.Infrastructure.Data.Accounts;

namespace Shreksoft.Bank.Infrastructure.Data;

public class BankDbContext(DbContextOptions<BankDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Transfer> Transfers { get; set; }

    // hand mapping for EF. Because EF can't map complex values like Money model etc.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // help EF to understand how to decompose complex types in fields
        modelBuilder.Entity<Account>(account =>
        {
            account.HasOne<Client>()
                .WithMany()
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            account.ComplexProperty(a => a.Money, MoneyConfiguration.Configure);
        });

        modelBuilder.Entity<Client>(client =>
        {
            client.ComplexProperty(c => c.Info, info =>
            {
                info.Property(i => i.Email);
                info.Property(i => i.PhoneNumber);
                info.ComplexProperty(i => i.BirthDate, birthDate =>
                {
                    birthDate.Property(d => d.Date);
                    birthDate.Ignore(d => d.Age);
                });
                info.ComplexProperty(i => i.FullName, fullName =>
                {
                    fullName.Property(n => n.FirstName);
                    fullName.Property(n => n.LastName);
                    fullName.Property(n => n.MiddleName);
                });
            });
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

// this class created only for EF works (migration etc. without EFCore.Design package in another csproj)
public class BankDbContextFactory : IDesignTimeDbContextFactory<BankDbContext>
{
    public BankDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<BankDbContext>()
            .UseNpgsql("FakeStringForMigration")
            .Options;

        return new BankDbContext(options);
    }
}
