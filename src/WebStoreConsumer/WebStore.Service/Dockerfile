#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

ENV DOTNET_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WebStoreConsumer/WebStore.Service/WebStore.Consumer.Service.csproj", "src/WebStoreConsumer/WebStore.Service/"]
COPY ["src/WebStoreConsumer/WebStore.EventHandling/WebStore.Consumer.RabbitMq.csproj", "src/WebStoreConsumer/WebStore.EventHandling/"]
COPY ["src/WebStoreCommon/WebStore.EventBus.Abstraction/WebStore.EventBus.Abstraction.csproj", "src/WebStoreCommon/WebStore.EventBus.Abstraction/"]
COPY ["src/WebStoreCommon/WebStore.Events/WebStore.Events.csproj", "src/WebStoreCommon/WebStore.Events/"]
COPY ["src/WebStoreCommon/WebStore.Extensions/WebStore.Extensions.csproj", "src/WebStoreCommon/Webstore.Extensions/"]
RUN dotnet restore "./src/WebStoreConsumer/WebStore.Service/./WebStore.Consumer.Service.csproj"
COPY . .
WORKDIR "/src/src/WebStoreConsumer/WebStore.Service"
RUN dotnet build "./WebStore.Consumer.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebStore.Consumer.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebStore.Consumer.Service.dll"]