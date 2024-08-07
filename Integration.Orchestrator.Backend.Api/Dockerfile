#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . ./
WORKDIR "/src"
RUN dotnet publish "./Integration.Orchestrator.Backend.Api/Integration.Orchestrator.Backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
EXPOSE 8081
EXPOSE 5052
EXPOSE 9171
ENTRYPOINT ["dotnet", "Integration.Orchestrator.Backend.Api.dll"]