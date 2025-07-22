FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app
EXPOSE 8081
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Facilities/Web.csproj ./Facilities/
COPY Application/Application.csproj ./Application/
COPY Entities/Entities.csproj ./Entities/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/

RUN dotnet restore ./Facilities/Web.csproj

COPY Facilities/ ./Facilities/
COPY Infrastructure/ ./Infrastructure/
COPY Entities/ ./Entities/
COPY Application/ ./Application/
WORKDIR /src/Facilities
RUN dotnet build "Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release 
RUN dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]