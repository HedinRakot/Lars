#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LarsProjekt/LarsProjekt.csproj", "LarsProjekt/"]
COPY ["LarsProjekt.Application/LarsProjekt.Application.csproj", "LarsProjekt.Application/"]
COPY ["LarsProjekt.Domain/LarsProjekt.Domain.csproj", "LarsProjekt.Domain/"]
COPY ["LarsProjekt.Database/LarsProjekt.Database.csproj", "LarsProjekt.Database/"]
RUN dotnet restore "./LarsProjekt/./LarsProjekt.csproj"
COPY . .
WORKDIR "/src/LarsProjekt"
RUN dotnet build "./LarsProjekt.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LarsProjekt.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LarsProjekt.dll"]