# Shrekoff Bank

[![.NET Build and Test](https://github.com/Shreksoft/shrekoff-bank/actions/workflows/dotnet.yaml/badge.svg?branch=main)](https://github.com/Shreksoft/shrekoff-bank/actions/workflows/dotnet.yaml)

![banner](docs/banner.png)

Banking system implemented in C#

## Stack

- C#
- PostgreSQL
- .NET 10
- xUnit
- ASP.NET Core Web API
- EF Core 10

## Getting Started

### Download repository

```shell
# ssh flow
git clone git@github.com:Shreksoft/shrekoff-bank.git && cd shrekoff-bank
```

### Database migrations

Migrations are applied automatically on startup, so no manual `dotnet ef database update` is needed

### Launching application

You can run app through [docker](#docker-start) or [.NET](#net-start)

#### Docker start

Copy the example environment file and adjust values if needed (`POSTGRES_PASSWORD` is required):

```shell
cp .env.example .env
# .env
# POSTGRES_DB=[your value]
# POSTGRES_USER=[your value]
# POSTGRES_PASSWORD=[your value] <- important
```

```shell
# the application starts on port 8080, database on 5432
docker compose up -d
```

#### .NET start
>[!Warning]
> For a local .NET run you need PostgreSQL running (locally or in Docker) on port 5432

>[!NOTE]
> You can change application port in `src/Shreksoft.Bank.Api/Properties/launchSettings.json` and set your database login/pass in `src/Shreksoft.Bank.Api/appsettings.Development.json`
>

> assuming the database is already running

```shell
# application will start on port 5294
dotnet restore
dotnet build --no-restore
dotnet run --project src/Shreksoft.Bank.Api/Shreksoft.Bank.Api.csproj
```

### Tests

```shell
dotnet test
```

## Misc

- More information about project (for not human) [here](AGENTS.md)
- Some architectural decisions (in Russian) [here](docs/adr.md)
