# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY global.json Directory.Build.props ./
COPY Socializer.Api/Socializer.Api.csproj Socializer.Api/
COPY Socializer.Application/Socializer.Application.csproj Socializer.Application/
COPY Socializer.Domain/Socializer.Domain.csproj Socializer.Domain/
COPY Socializer.Infrastructure/Socializer.Infrastructure.csproj Socializer.Infrastructure/
COPY Socializer.Contracts/Socializer.Contracts.csproj Socializer.Contracts/
RUN dotnet restore Socializer.Api/Socializer.Api.csproj

COPY Socializer.Api/ Socializer.Api/
COPY Socializer.Application/ Socializer.Application/
COPY Socializer.Domain/ Socializer.Domain/
COPY Socializer.Infrastructure/ Socializer.Infrastructure/
COPY Socializer.Contracts/ Socializer.Contracts/
RUN dotnet publish Socializer.Api/Socializer.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Socializer.Api.dll"]
