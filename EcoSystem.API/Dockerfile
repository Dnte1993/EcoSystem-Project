# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo del proyecto y restaurar dependencias
COPY ["EcoSystem.API.csproj", "./"]
RUN dotnet restore "./EcoSystem.API.csproj"

# Copiar el resto del código y compilar
COPY . .
RUN dotnet publish "EcoSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa de ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EcoSystem.API.dll"]