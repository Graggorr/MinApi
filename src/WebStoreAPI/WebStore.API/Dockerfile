#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production
ENV PORT=8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WebStoreAPI/WebStore.API/WebStore.API.Service.csproj", "src/WebStoreAPI/WebStore.API/"]
COPY ["src/WebStoreAPI/WebStore.Application/WebStore.API.Application.csproj", "src/WebStoreAPI/WebStore.Application/"]
COPY ["src/WebStoreAPI/WebStore.Domain/WebStore.API.Domain.csproj", "src/WebStoreAPI/WebStore.Domain/"]
COPY ["src/WebStoreAPI/WebStore.Infrastructure/WebStore.API.Infrastructure.csproj", "src/WebStoreAPI/WebStore.Infrastructure/"]
COPY ["src/WebStoreCommon/WebStore.Events/WebStore.Events.csproj", "src/WebStoreCommon/WebStore.Events/"]
COPY ["src/WebStoreCommon/WebStore.EventBus.Abstraction/WebStore.EventBus.Abstraction.csproj", "src/WebStoreCommon/WebStore.EventBus.Abstraction/"]
COPY ["src/WebStoreCommon/WebStore.Extensions/WebStore.Extensions.csproj", "src/WebStoreCommon/Webstore.Extensions/"]
RUN dotnet restore "./src/WebStoreAPI/WebStore.API/./WebStore.API.Service.csproj"
COPY . .
WORKDIR "/src/src/WebStoreAPI/WebStore.API"
RUN dotnet build "./WebStore.API.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebStore.API.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebStore.API.Service.dll"]