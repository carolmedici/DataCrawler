FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DataCrawler.API/DataCrawler.API.csproj", "DataCrawler.API/"]
RUN dotnet restore "DataCrawler.API/DataCrawler.API.csproj"
COPY . .
WORKDIR "/src/DataCrawler.API"
RUN dotnet build "DataCrawler.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataCrawler.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataCrawler.API.dll"]