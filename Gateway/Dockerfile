FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Gateway.sln .
COPY Gateway/Gateway.csproj Gateway/
RUN dotnet restore Gateway.sln

COPY . .

WORKDIR /src/Gateway

RUN dotnet publish "Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8084

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Gateway.dll"]
