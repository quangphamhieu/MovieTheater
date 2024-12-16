# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MovieTheater/MovieTheater.csproj", "MovieTheater/"]
COPY ["Services/MovieTheater/MT.Applicationservices/MT.Applicationservices.csproj", "Services/MovieTheater/MT.Applicationservices/"]
COPY ["Services/Shared/MT.Shared.Constant/MT.Shared.Constant.csproj", "Services/Shared/MT.Shared.Constant/"]
COPY ["Services/MovieTheater/MT.Dtos/MT.Dtos.csproj", "Services/MovieTheater/MT.Dtos/"]
COPY ["Services/MovieTheater/MT.Domain/MT.Domain.csproj", "Services/MovieTheater/MT.Domain/"]
COPY ["Services/MovieTheater/MT.Infrastructure/MT.Infrastructure.csproj", "Services/MovieTheater/MT.Infrastructure/"]
RUN dotnet restore "./MovieTheater/MovieTheater.csproj"
COPY . .
WORKDIR "/src/MovieTheater"
RUN dotnet build "./MovieTheater.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MovieTheater.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieTheater.dll"]