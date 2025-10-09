FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug

RUN apt-get update && apt-get install -y \
    libgdiplus \
    libc6-dev \
    libx11-dev \
    libfontconfig1 \
    libfreetype6 \
    libgtk2.0-0 \
    libnss3 \
    libatk-bridge2.0-0 \
    libx11-xcb1 \
    libxcb-dri3-0 \
    libdrm-common \
    libgbm1 \
    libasound2 \
    libappindicator3-1 \
    libxrender1 \
    libfontconfig1 \
    libxshmfence1 \
    libgdiplus \
    libva-dev \
    libglib2.0-0 \
    --no-install-recommends


WORKDIR /src

COPY ["Users.sln", "."]
COPY ["Presentation/Presentation.csproj", "Presentation/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["ExceptionHandler/ExceptionHandler.csproj", "ExceptionHandler/"]

RUN dotnet restore "Users.sln"

COPY . .
WORKDIR "/src/Presentation"
RUN dotnet publish "Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
USER root
COPY --from=build /app/publish .
RUN chown -R app:app /app

ENTRYPOINT ["dotnet", "Presentation.dll"]