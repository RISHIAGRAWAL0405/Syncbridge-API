FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY PhotographyPortfolioAPI.sln ./
COPY PhotographyPortfolioAPI/PhotographyPortfolioAPI.csproj PhotographyPortfolioAPI/
COPY PhotographyPortfolioAPI.Core/PhotographyPortfolioAPI.Core.csproj PhotographyPortfolioAPI.Core/
COPY PhotographyPortfolioAPI.Contracts/PhotographyPortfolioAPI.Contracts.csproj PhotographyPortfolioAPI.Contracts/
COPY PhotographyPortfolioAPI.Infrastructure/PhotographyPortfolioAPI.Infrastructure.csproj PhotographyPortfolioAPI.Infrastructure/
RUN dotnet restore PhotographyPortfolioAPI.sln

COPY . ./
RUN dotnet publish PhotographyPortfolioAPI/PhotographyPortfolioAPI.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 10000
ENTRYPOINT ["sh", "-c", "dotnet PhotographyPortfolioAPI.dll --urls http://0.0.0.0:${PORT:-10000}"]
