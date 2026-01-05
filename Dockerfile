FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["API/SmartStoreReservation.API.csproj", "API/"]
COPY ["Core/SmartStoreReservation.Core.csproj", "Core/"]
COPY ["Data/SmartStoreReservation.Data.csproj", "Data/"]
COPY ["Services/SmartStoreReservation.Services.csproj", "Services/"]

# Restore dependencies
RUN dotnet restore "API/SmartStoreReservation.API.csproj"

# Copy source code
COPY . .

# Build
WORKDIR "/src/API"
RUN dotnet build "SmartStoreReservation.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartStoreReservation.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "SmartStoreReservation.API.dll"]
