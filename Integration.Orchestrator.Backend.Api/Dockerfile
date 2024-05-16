#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GrupoKFC.MaxPoint.Integration.Api/GrupoKFC.MaxPoint.Integration.Api.csproj", "GrupoKFC.MaxPoint.Integration.Api/"]
RUN dotnet restore "./GrupoKFC.MaxPoint.Integration.Api/GrupoKFC.MaxPoint.Integration.Api.csproj"
COPY . .
WORKDIR "/src/GrupoKFC.MaxPoint.Integration.Api"
RUN dotnet build "./GrupoKFC.MaxPoint.Integration.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GrupoKFC.MaxPoint.Integration.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrupoKFC.MaxPoint.Integration.Api.dll"]