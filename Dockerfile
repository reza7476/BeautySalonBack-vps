# ============================
# Stage 1 : Build
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# ---------- Copy Project Files ----------
COPY src/00.commons/BeautySalon.Common/BeautySalon.Common.csproj src/00.commons/BeautySalon.Common/
COPY src/01.core/BeautySalon.Application/BeautySalon.Application.csproj src/01.core/BeautySalon.Application/
COPY src/01.core/BeautySalon.Entities/BeautySalon.Entities.csproj src/01.core/BeautySalon.Entities/
COPY src/01.core/BeautySalon.Services/BeautySalon.Services.csproj src/01.core/BeautySalon.Services/
COPY src/02.infrastructure/BeautySalon.infrastructure/BeautySalon.infrastructure.csproj src/02.infrastructure/BeautySalon.infrastructure/
COPY src/03.RestApi/BeautySalon.RestApi/BeautySalon.RestApi.csproj src/03.RestApi/BeautySalon.RestApi/

# ---------- Restore ----------
RUN dotnet restore "src/03.RestApi/BeautySalon.RestApi/BeautySalon.RestApi.csproj"

# ---------- Copy Source ----------
COPY src src

# ---------- Publish ----------
RUN dotnet publish "src/03.RestApi/BeautySalon.RestApi/BeautySalon.RestApi.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# ============================
# Stage 2 : Runtime
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BeautySalon.RestApi.dll"]