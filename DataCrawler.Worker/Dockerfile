FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DataCrawler.Worker/DataCrawler.Worker.csproj", "DataCrawler.Worker/"]
RUN dotnet restore "DataCrawler.Worker/DataCrawler.Worker.csproj"
COPY . .
WORKDIR "/src/DataCrawler.Worker"
RUN dotnet build "DataCrawler.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataCrawler.Worker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataCrawler.Worker.dll"]