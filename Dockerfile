FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DistanceCalculator/DistanceCalculator.csproj DistanceCalculator/
RUN dotnet restore DistanceCalculator/DistanceCalculator.csproj

COPY . .
RUN dotnet publish DistanceCalculator/DistanceCalculator.csproj -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /publish .

RUN chown -R app /app
USER app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "DistanceCalculator.dll"]
