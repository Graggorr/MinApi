#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082

ENV DOTNET_ENVIRONMENT=Production
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WebStoreBus/WebStore.EventBus.Service/WebStore.EventBus.Service.csproj", "src/WebStoreBus/WebStore.EventBus.Service/"]
COPY ["src/WebStoreBus/WebStore.EventBus.BackgroundJobService/WebStore.EventBus.BackgroundJobService.csproj", "src/WebStoreBus/WebStore.EventBus.BackgroundJobService/"]
COPY ["src/WebStoreCommon/WebStore.Events/WebStore.Events.csproj", "src/WebStoreCommon/WebStore.Events/"]
COPY ["src/WebStoreCommon/WebStore.EventBus.Abstraction/WebStore.EventBus.Abstraction.csproj", "src/WebStoreCommon/WebStore.EventBus.Abstraction/"]
COPY ["src/WebStoreBus/WebStore.EventBus.RabbitMq/WebStore.EventBus.RabbitMq.csproj", "src/WebStoreBus/WebStore.EventBus.RabbitMq/"]
COPY ["src/WebStoreCommon/WebStore.Extensions/WebStore.Extensions.csproj", "src/WebStoreCommon/Webstore.Extensions/"]
RUN dotnet restore "./src/WebStoreBus/WebStore.EventBus.Service/./WebStore.EventBus.Service.csproj"
COPY . .
WORKDIR "/src/src/WebStoreBus/WebStore.EventBus.Service"
RUN dotnet build "./WebStore.EventBus.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebStore.EventBus.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebStore.EventBus.Service.dll"]