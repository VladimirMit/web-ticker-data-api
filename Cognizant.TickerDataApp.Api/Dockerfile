#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Cognizant.TickerDataApp.Api/Cognizant.TickerDataApp.Api.csproj", "Cognizant.TickerDataApp.Api/"]
RUN dotnet restore "Cognizant.TickerDataApp.Api/Cognizant.TickerDataApp.Api.csproj"
COPY . .
WORKDIR "/src/Cognizant.TickerDataApp.Api"
RUN dotnet build "Cognizant.TickerDataApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cognizant.TickerDataApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cognizant.TickerDataApp.Api.dll"]