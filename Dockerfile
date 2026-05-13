# Dockerfile para ASP.NET 8 - WalletWeb
# IMPORTANTE: Este Dockerfile debe estar en: /HomeSolido/Media/Repos/Programas NET/Wallet/
# Ejecutar desde ese directorio: docker build -t tuusuario/walletweb:latest .

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto para restaurar dependencias
COPY Application.Interfaces/Application.csproj ./Application.Interfaces/
COPY Domain.Entity/Domain.Model.csproj ./Domain.Entity/
COPY ExternalServices/Infra.ExternalServices.csproj ./ExternalServices/
COPY Infra.DataAccess/Infra.DataAccess.csproj ./Infra.DataAccess/
COPY Shared/Shared.csproj ./Shared/
COPY WalletWeb/UI.WalletWeb.csproj ./WalletWeb/
COPY WalletWeb/WalletWeb.sln ./WalletWeb/

# Restaurar dependencias
RUN dotnet restore ./WalletWeb/WalletWeb.sln

# Copiar todo el código fuente
COPY Application.Interfaces/ ./Application.Interfaces/
COPY Domain.Entity/ ./Domain.Entity/
COPY ExternalServices/ ./ExternalServices/
COPY Infra.DataAccess/ ./Infra.DataAccess/
COPY Shared/ ./Shared/
COPY WalletWeb/ ./WalletWeb/

# Compilar y publicar la aplicación
WORKDIR /src/WalletWeb
RUN dotnet publish UI.WalletWeb.csproj -c Release -o /app/publish

# Imagen runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /app/publish .

# Exponer el puerto
EXPOSE 8080
EXPOSE 8081

# Variable de entorno para la URL
ENV ASPNETCORE_URLS=http://+:8080

# Comando de inicio
ENTRYPOINT ["dotnet", "UI.WalletWeb.dll"]