FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR app
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release
COPY RouteService.sln .
COPY Presentation/Presentation.csproj ./Presentation/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY Application/Application.csproj ./Application/
COPY Entities/Entities.csproj ./Entities/
COPY ExceptionHandler/ExceptionHandler.csproj ./ExceptionHandler/
RUN dotnet restore RouteService.sln

COPY Presentation/ ./Presentation/
COPY Infrastructure/ ./Infrastructure/
COPY Application/ ./Application/
COPY Entities/ ./Entities/
COPY ExceptionHandler/ ./ExceptionHandler/

WORKDIR /src/Presentation
RUN dotnet build "Presentation.csproj" -c $BUILD_CONFIGURATION -o app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release 
RUN dotnet publish "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]