FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["LuzyceWebApp/LuzyceWebApp.csproj", "LuzyceWebApp/"]
RUN dotnet restore "./LuzyceWebApp/LuzyceWebApp.csproj"

COPY . .
WORKDIR "/src/LuzyceWebApp"
RUN dotnet build "./LuzyceWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LuzyceWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 35133

RUN pwd
RUN ls -la
COPY --from=publish /app/publish .
RUN ls -la

ENTRYPOINT ["dotnet", "LuzyceWebApp.dll"]

