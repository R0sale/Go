FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Presentation/Presentation.csproj ./Presentation/
COPY Application/Application.csproj ./Application/
COPY Entities/Entities.csproj ./Entities/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
RUN dotnet restore ./Presentation/Presentation.csproj
COPY Presentation/ ./Presentation/
COPY Infrastructure/ ./Infrastructure/
COPY Entities/ ./Entities/
COPY Application/ ./Application/
WORKDIR /src/Presentation
RUN dotnet build "Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release 
RUN dotnet publish "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]