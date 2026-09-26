# Stage 1
# final runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
EXPOSE 8080
WORKDIR /app
RUN mkdir ./data

# Stage 2
# code compilation
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build
# copy nu-get manifests (for docker layer cache)
COPY ["src/Shreksoft.Bank.Api/Shreksoft.Bank.Api.csproj", "src/Shreksoft.Bank.Api/"]
COPY ["src/Shreksoft.Bank.Core/Shreksoft.Bank.Core.csproj", "src/Shreksoft.Bank.Core/"]
COPY ["src/Shreksoft.Bank.Infrastructure/Data/Shreksoft.Bank.Infrastructure.Data.csproj", "src/Shreksoft.Bank.Infrastructure/Data/"]
COPY ["src/Shreksoft.Bank.Application/Shreksoft.Bank.Application.csproj", "src/Shreksoft.Bank.Application/"]
RUN dotnet restore "src/Shreksoft.Bank.Api/Shreksoft.Bank.Api.csproj"
COPY . .
RUN dotnet build src/Shreksoft.Bank.Api -c $BUILD_CONFIGURATION --no-restore

# Stage 3
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish src/Shreksoft.Bank.Api -c $BUILD_CONFIGURATION --no-build -o /app/publish /p:UseAppHost=false

# Stage 4
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shreksoft.Bank.Api.dll"]
