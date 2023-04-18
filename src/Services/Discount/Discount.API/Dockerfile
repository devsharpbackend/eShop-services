#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src


COPY ["eShop-services.sln". "eShop-services.sln"]
COPY ["src/Services/Discount/Discount.API/Discount.API.csproj", "src/Services/Discount/Discount.API/"]
COPY ["BuildingBlocks/Logging/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Logging/Common.Logging/"]
COPY ["BuildingBlocks/Host/Common.Host/Common.Host.csproj", "BuildingBlocks/Host/Common.Host/"]
COPY ["src/Services/Catalog/Catalog.Domain/Catalog.Domain.csproj", "src/Services/Catalog/Catalog.Domain/"]
COPY ["src/Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "src/Services/Catalog/Catalog.Infrastructure/"]
COPY ["BuildingBlocks/Event/Common.Event/Common.Event.csproj", "BuildingBlocks/Event/Common.Event/"]
COPY ["BuildingBlocks/Event/ZeroMqEvent/ZeroMqEvent.csproj", "BuildingBlocks/Event/ZeroMqEvent/"]

#RUN dotnet restore "src/Services/Catalog/Catalog.API/Catalog.API.csproj"


COPY "docker-compose.dcproj" "docker-compose.dcproj"

COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "eShop-services.sln"

COPY . .
WORKDIR /src/Services/Discount/Discount.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=build /src/Services/Discount/Discount.API/Proto /app/Proto
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Discount.API.dll"]
