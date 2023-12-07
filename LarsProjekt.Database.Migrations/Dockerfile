#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LarsProjekt.Database.Migrations/LarsProjekt.Database.Migrations.csproj", "LarsProjekt.Database.Migrations/"]
RUN dotnet restore "./LarsProjekt.Database.Migrations/./LarsProjekt.Database.Migrations.csproj"
COPY . .
WORKDIR "/src/LarsProjekt.Database.Migrations"
RUN dotnet build "./LarsProjekt.Database.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LarsProjekt.Database.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LarsProjekt.Database.Migrations.dll"]