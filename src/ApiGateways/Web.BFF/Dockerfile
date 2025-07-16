FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/ApiGateways/Web.BFF/Web.BFF.csproj", "src/ApiGateways/Web.BFF/"]
COPY ["src/BuildingBlocks/Common/Common.csproj", "src/BuildingBlocks/Common/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/"]

RUN dotnet restore "src/ApiGateways/Web.BFF/Web.BFF.csproj"

COPY . .
WORKDIR "/src/src/ApiGateways/Web.BFF"
RUN dotnet build "Web.BFF.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.BFF.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.BFF.dll"]