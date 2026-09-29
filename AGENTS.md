# Shreksoft.Bank — Core Banking System (learning project)

## Stack

- C#, .NET 10 (`net10.0`), xUnit tests, Moq
- ASP.NET Core Web API host (`Shreksoft.Bank.Api`)
- PostgreSQL via EF Core 10 (`Npgsql.EntityFrameworkCore.PostgreSQL` + EF Core `.Design` in Infrastructure)
- Migrations applied automatically at startup (`Database.MigrateAsync` in `Program.cs`) or via EF CLI; the schema is a
  single consolidated Postgres migration (`InitialCreate`)
- Rate provider is in-memory (`InMemoryConvertRateProvider`); real repositories are EF-based
- Containerization: single root `Dockerfile` (multi-stage, `USER $APP_UID`, FDD publish, port 8080) + `compose.yaml`
  with `shrekoff-bank-api` and `shrekoff-bank-db` (postgres:18, healthcheck, `pg-data` volume); DB credentials come
  from `.env` (`POSTGRES_DB`/`POSTGRES_USER`/`POSTGRES_PASSWORD`)
- CI: `.github/workflows/dotnet.yaml` — build + test on push to `main`/`dev` and on PRs (tests are unit-level, no DB needed)
- DI via `Microsoft.Extensions.DependencyInjection`: `AddData()` extension (Infrastructure) + `AddScoped` (Api/`Program.cs`); composition root is `Program.cs`
- EditorConfig: indent 4 spaces for `*.cs`

## Solution structure

- `src/Shreksoft.Bank.Core/` — pure domain, no services (namespaces are `Shreksoft.Bank.Core.Domain.*`):
  - `Domain/Accounts/` — `Account`, `Money` + `Exceptions/` (domain exceptions)
  - `Domain/Clients/` — `Client`, `ClientInfo`, `FullName`, `BirthDate`
  - `Domain/Shared/Currencies/` — `Currency`, `CurrencyCode`
  - `Domain/Transfers/` — `Transfer`, `TransferSide` (immutable money-transfer snapshot)
- `src/Shreksoft.Bank.Application/` — application layer:
  - `Accounts/` — `AccountService`, `IAccountRepository`, `ITransferRepository`, `IConvertRateProvider`
  - `Clients/` — `ClientService`, `IClientRepository`
  - `IUnitOfWork`; `Shared/ObjectNotFoundException`
- `src/Shreksoft.Bank.Infrastructure/Data/` — EF Core persistence:
  - `BankDbContext` (+ design-time `BankDbContextFactory`, Postgres), `UnitOfWork`, `DependencyInjection` (`AddData`)
  - `Accounts/`, `Clients/`, `Transfers/` — EF implementations of application interfaces and entity configs
  - `Providers/Rates/InMemoryConvertRateProvider`
  - `Migrations/` — one Postgres migration (`20260928133925_InitialCreate`) + snapshot
- `src/Shreksoft.Bank.Api/` — hosting:
  - `Program.cs` (composition root): `AddData` + `AddScoped<AccountService/ClientService>`, `MigrateAsync` at startup;
    local run reads `appsettings.Development.json` (Postgres on `localhost:5432`, db `shrekoff-bank-db`); http profile = port 5294
  - `Controllers/` — `BaseApiController` (`[ApiController]`, `Route("api/[controller]")`), `Accounts/`, `Clients/` + `Dto/`
  - `Middlewares/ExceptionMiddleware` — maps domain exceptions to 400/404/500 JSON
- `tests/Shreksoft.Bank.Core.Tests/` — xUnit + Moq:
  - `Accounts/` — `Domain/AccountTests`, `Services/AccountServiceTests`
  - `Clients/` — `Domain/BirthDateTests`, `Domain/FullNameTests`, `Services/ClientServiceTests`

## Commands

```
dotnet build
dotnet test
dotnet run --project src/Shreksoft.Bank.Api/Shreksoft.Bank.Api.csproj   # local dev, port 5294
dotnet ef migrations add <Name>        # migration lands in Infrastructure/Data/Migrations
dotnet ef database update              # applies migrations (needs Postgres on 5432)

docker compose up -d                   # api (8080) + postgres (5432); use `podman compose` with Podman
```

## Notable conventions

- **Account** (`public`, in namespace `Shreksoft.Bank.Core.Domain.Accounts`): public creation ctor validating
  `Currency.IsValid` + private parameterless ctor for EF; properties `init`/`private set`; state changes via `Debit`,
  `Credit`, `Block`, `Unblock`; balances rounded to `Currency.Scale`; invalid operations throw domain exceptions.
- **Transfer**: immutable snapshot of a money-transfer event (`class`, no mutation methods). Constructor validates
  invariants (no self-transfer, `rate`/`amount` non-positive), computes `Commission` (`0.02m`, zero when both sides
  share the same `ClientId`) and rounds `RecipientAmount` by `Currency.Scale`. `Rate` is `decimal`.
- **Value objects are `readonly record struct`** (`Money`, `ClientInfo`, `FullName`, `TransferSide`; `BirthDate` is a
  `readonly struct`) with a default ctor for EF; properties `init`, mutated via `with` expressions.
- **Client** is `public`, created through `ClientService.CreateClient(ClientInfo)`.
- **Domain exceptions** in `accounts` exceptions folder are `sealed` primary-constructor exceptions with named
  properties (e.g. `AccountBlockedException(Id)`, `InsufficientFundsException(Id, Balance)`).
- **Application services** (`AccountService`, `ClientService`) are primary-constructor classes with injected repos;
  `XxxByIdOrThrow(...)` returns the entity or throws `ObjectNotFoundException`; a unit of work is committed via
  `IUnitOfWork.SaveChanges()`.
- **Persistence** is EF Core: repository implementations in `Shreksoft.Bank.Infrastructure.Data.*` wrap `BankDbContext` (`Find`,
  `Add`). `BankDbContext.OnModelCreating` maps value objects via `ComplexProperty` (e.g. `Money.Currency`, client
  `Info`, transfer sides), `Account`→`Client` FK with `DeleteBehavior.Restrict`, `CurrencyCode` via
  `HasConversion<string>`; `TransferConfiguration` is applied via `ApplyConfigurationsFromAssembly`.
- **Namespaces are canonical**: `Accounts`, `Clients`, `Transfers`, `Shared.Currencies` all live under
  `Shreksoft.Bank.Core.Domain.*`; `using`-directions and EF migration snapshot must stay in sync.
- **Tests** use xUnit (`[Fact]`, `[Theory]`, `Assert.Throws<T>`) and Moq (`Mock<I***Repository>`, `IUnitOfWork`);
  domain tests exercise real domain classes.

## Роль

Ты — мой ментор и код-ревьюер, senior software architect.
Я — начинающий разработчик, учусь проектировать core banking систему.

## Правила поведения

1. НИКОГДА не пиши код за меня и не предлагай готовые реализации,
   если я явно не попросил показать пример синтаксиса.
