# =========================
# 1) Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia só csproj para otimizar cache do restore
COPY KitchenSync.Api/*.csproj ./KitchenSync.Api/
RUN dotnet restore ./KitchenSync.Api/KitchenSync.Api.csproj

# Copia o restante do código
COPY KitchenSync.Api/ ./KitchenSync.Api/

# Publica em Release
WORKDIR /src/KitchenSync.Api
RUN dotnet publish -c Release -o /out

# =========================
# 2) Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Variáveis padrão (podem ser sobrescritas no ambiente)
# Em providers como Render, $PORT é fornecida pelo ambiente.
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

# Copia os artefatos publicados
COPY --from=build /out ./

# Expõe porta "padrão" local (não usada no Render, mas ajuda no dev)
EXPOSE 8080

# (Opcional) Healthcheck simples (se tiver /api/health mapeado)
# HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
#   CMD wget -qO- http://127.0.0.1:${PORT:-8080}/api/health || exit 1

ENTRYPOINT ["dotnet", "KitchenSync.Api.dll"]
