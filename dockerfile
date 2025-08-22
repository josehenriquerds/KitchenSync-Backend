# =========================
# 1) Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Otimiza cache
COPY KitchenSync.Api/*.csproj ./KitchenSync.Api/
RUN dotnet restore ./KitchenSync.Api/KitchenSync.Api.csproj

# Copia o restante
COPY KitchenSync.Api/ ./KitchenSync.Api/

# Publica em Release (self-contained opcional)
WORKDIR /src/KitchenSync.Api
RUN dotnet publish -c Release -o /out

# =========================
# 2) Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia artefatos
COPY --from=build /out ./

# Exposição local (no Render é ignorada, mas ajuda no dev)
EXPOSE 8080

# Usa bash para interpolar $PORT em runtime. Default 8080 localmente.
ENTRYPOINT [ "bash", "-c", "dotnet KitchenSync.Api.dll --urls http://0.0.0.0:${PORT:-8080}" ]
