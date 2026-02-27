FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Copiar el .csproj específico usando la ruta desde la raíz
COPY ["Gateway/Service.Gateway/Service.Gateway.csproj", "Gateway/Service.Gateway/"]

# 2. Restaurar dependencias
RUN dotnet restore "Gateway/Service.Gateway/Service.Gateway.csproj"

# 3. Copiar todo el código de la solución
COPY . .

# 4. Cambiar al directorio del proyecto y compilar
WORKDIR "/src/Gateway/Service.Gateway"
RUN dotnet build "Service.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Service.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.Gateway.dll"]