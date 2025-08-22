# ============ 1) Build ============
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia só o csproj p/ cache do restore
COPY *.csproj ./
RUN dotnet restore ./KitchenSync.Api.csproj

# Copia o restante do código (exceto o que estiver no .dockerignore)
COPY . ./

# Publica em Release
RUN dotnet publish ./KitchenSync.Api.csproj -c Release -o /out

# ============ 2) Runtime ==========
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia artefatos publicados
COPY --from=build /out ./

# Exposição local (Render ignora, mas ajuda no dev)
EXPOSE 8080

# Interpola $PORT em runtime (Render define PORT)
ENTRYPOINT ["bash", "-c", "dotnet KitchenSync.Api.dll --urls http://0.0.0.0:${PORT:-8080}"]
