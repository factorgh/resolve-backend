# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Image for building the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore as distinct layers
COPY ["src/ResolveBridge.Api/ResolveBridge.Api.csproj", "src/ResolveBridge.Api/"]
COPY ["src/ResolveBridge.Application/ResolveBridge.Application.csproj", "src/ResolveBridge.Application/"]
COPY ["src/ResolveBridge.Domain/ResolveBridge.Domain.csproj", "src/ResolveBridge.Domain/"]
COPY ["src/ResolveBridge.Infrastructure/ResolveBridge.Infrastructure.csproj", "src/ResolveBridge.Infrastructure/"]

RUN dotnet restore "src/ResolveBridge.Api/ResolveBridge.Api.csproj"

# Copy the remaining source code
COPY . .
WORKDIR "/src/src/ResolveBridge.Api"

# Build the application
RUN dotnet build "ResolveBridge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ResolveBridge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResolveBridge.Api.dll"]
