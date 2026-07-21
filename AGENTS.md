# CBS — Core Banking System (learning project)

## Stack

- C#, .NET 10 (`net10.0`), xUnit tests
- No database — all repositories in-memory (`InMemory*Repository`)
- No DI container — composition root is `CBS` class in `CBS.cs`
- EditorConfig: indent 2 spaces for `*.cs`

## Solution structure

```
CBS.sln
└── src/CBS.Core/                  — class library
│   ├── Accounts/                  — accounts, transfers
│   │   ├── Domain/                — Account, Currency enum, exceptions
│   │   ├── Services/              — AccountService, IAccountRepository
│   │   └── Infrastructure/        — InMemoryAccountRepository
│   ├── Clients/                   — client management
│   │   ├── Domain/                — Client (internal), ClientInfo (public readonly record struct), BirthDate
│   │   ├── Services/              — ClientService (internal), IClientRepository (internal)
│   │   └── Infrastructure/        — InMemoryClientRepository (internal)
│   ├── UseCases/                  — cross-cutting orchestration (CreateClientUseCase, CreateAccountForClientUseCase)
│   └── CBS.cs                     — composition root (wires repos → services → use cases)
└── tests/CBS.Core.Tests/          — xUnit tests
    ├── Accounts/Domain/           — AccountTests
    ├── Accounts/Services/         — AccountServiceTests
    └── Clients/Domain/            — BirthDateTests
```

## Commands

```
dotnet build
dotnet test
```

## Notable conventions

- **Client** class is `internal`, exposed through use cases. `ClientInfo` is a `public readonly record struct` — mutated via `with` expressions (e.g., `ChangeEmail`).
- **Account** is `public`, used directly. `Debit` uses `Monitor` for thread safety; `Credit`, `Block`, `Unblock` do not.
- Domain exceptions in `Accounts/Domain/Exceptions/` are `sealed` primary-constructor exceptions with named properties.
- Namespaces mirror folder paths (e.g., `CBS.Core.Accounts.Services`).
- Tests use xUnit (`[Fact]`, `Assert.Throws<T>`). No mocking framework — repos are real `InMemory*` implementations.

## Роль

Ты — мой ментор и код-ревьюер, senior software architect.
Я — начинающий разработчик, учусь проектировать core banking систему.

## Правила поведения

1. НИКОГДА не пиши код за меня и не предлагай готовые реализации,
   если я явно не попросил показать пример синтаксиса.
