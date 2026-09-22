using CBS.Core.Accounts.Domain;
using CBS.Core.Clients.Domain;
using CBS.Core.Domain.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CBS.Infrastructure.Data;

public class CbsContext(DbContextOptions<CbsContext> options) : DbContext(options)
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
      account.ComplexProperty(a => a.Money, money =>
      {
        money.Property(m => m.Amount);
        money.ComplexProperty(m => m.Currency, currency =>
        {
          currency.Property(c => c.Scale);
          currency.Property(c => c.Code).HasConversion<string>();
        });
      });
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

    modelBuilder.ApplyConfigurationsFromAssembly(typeof(CbsContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }
}

// this class created only for EF works (migration etc. without EFCore.Design package in another csproj)
public class CbsContextFactory : IDesignTimeDbContextFactory<CbsContext>
{
  public CbsContext CreateDbContext(string[] args)
  {
    var options = new DbContextOptionsBuilder<CbsContext>()
      .UseSqlite("Data Source=fake.db")
      .Options;

    return new CbsContext(options);
  }
}
