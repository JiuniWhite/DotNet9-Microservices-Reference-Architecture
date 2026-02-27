FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copiamos los proyectos manteniendo la carpeta "Proveedores"
# Esto hace que las referencias "../../" de los .csproj funcionen
COPY ["Proveedores/Provider.Api/Provider.Api.csproj", "Proveedores/Provider.Api/"]
COPY ["Proveedores/Provider.Application/Provider.Application.csproj", "Proveedores/Provider.Application/"]
COPY ["Proveedores/Provider.Infrastructure/Provider.Infrastructure.csproj", "Proveedores/Provider.Infrastructure/"]
COPY ["Proveedores/Provider.Core/Provider.Core.csproj", "Proveedores/Provider.Core/"]

# 2. Restauramos
RUN dotnet restore "Proveedores/Provider.Api/Provider.Api.csproj"

# 3. Copiamos TODO el contenido respetando la estructura
COPY . .

# 4. Compilamos
# Al estar en /src, el compilador verá /src/Proveedores/Provider.Application etc.
RUN dotnet build "Proveedores/Provider.Api/Provider.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Proveedores/Provider.Api/Provider.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Provider.Api.dll"]