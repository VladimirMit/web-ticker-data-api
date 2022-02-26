#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Company.TickerDataApp.Api/Company.TickerDataApp.Api.csproj", "Company.TickerDataApp.Api/"]
RUN dotnet restore "Company.TickerDataApp.Api/Company.TickerDataApp.Api.csproj"
COPY . .
WORKDIR "/src/Company.TickerDataApp.Api"
RUN dotnet build "Company.TickerDataApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Company.TickerDataApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.TickerDataApp.Api.dll"]