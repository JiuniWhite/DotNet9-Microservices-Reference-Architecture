FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copiamos los proyectos manteniendo la carpeta "Catalogo"
COPY ["Catalogo/Catalog.Api/Catalog.Api.csproj", "Catalogo/Catalog.Api/"]
COPY ["Catalogo/Catalog.Application/Catalog.Application.csproj", "Catalogo/Catalog.Application/"]
COPY ["Catalogo/Catalog.Core/Catalog.Core.csproj", "Catalogo/Catalog.Core/"]
COPY ["Catalogo/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Catalogo/Catalog.Infrastructure/"]

# 2. Restauramos
RUN dotnet restore "Catalogo/Catalog.Api/Catalog.Api.csproj"

# 3. Copiamos TODO el contenido respetando la estructura
COPY . .

# 4. Compilamos
RUN dotnet build "Catalogo/Catalog.Api/Catalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalogo/Catalog.Api/Catalog.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]