# Etapa de compilación: SDK, restauración de paquetes y publicación Release.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["G4AspNet.csproj", "./"]
RUN dotnet restore "G4AspNet.csproj"

COPY . .
RUN dotnet publish "G4AspNet.csproj" --configuration Release --output /app/publish /p:UseAppHost=false

# Imagen final pequeña: solo contiene el runtime de ASP.NET Core.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
RUN mkdir -p App_Data wwwroot/uploads/avatars

EXPOSE 10000

# Render define PORT; 10000 permite ejecutar también el contenedor localmente.
ENTRYPOINT ["sh", "-c", "exec dotnet G4AspNet.dll --urls http://0.0.0.0:${PORT:-10000}"]
