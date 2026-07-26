# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiamos todo el repositorio completo (API, Business, Data) al contenedor
COPY . .

# Nos movemos a la carpeta de la API para compilar desde ahí
WORKDIR /src/EcoSystem.API
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa de ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EcoSystem.API.dll"]