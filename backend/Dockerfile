# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar archivos de proyecto
COPY ["src/Api/StudentRegistration.Api.csproj", "src/Api/"]
COPY ["src/Application/StudentRegistration.Application.csproj", "src/Application/"]
COPY ["src/Domain/StudentRegistration.Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/StudentRegistration.Infrastructure.csproj", "src/Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "src/Api/StudentRegistration.Api.csproj"

# Copiar todo el código fuente
COPY . .

# Build de la aplicación
RUN dotnet build "src/Api/StudentRegistration.Api.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "src/Api/StudentRegistration.Api.csproj" -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copiar archivos publicados desde la etapa de build
COPY --from=build /app/publish .

# Copiar script de entrada
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# Exponer puerto
EXPOSE 8080

# Variables de entorno por defecto
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Comando de inicio
ENTRYPOINT ["/app/entrypoint.sh"]
