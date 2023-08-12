#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Food Analyzer/FoodAnalyzer.API/FoodAnalyzer.API.csproj", "Food Analyzer/FoodAnalyzer.API/"]
RUN dotnet restore "Food Analyzer/FoodAnalyzer.API/FoodAnalyzer.API.csproj"
COPY . .
WORKDIR "/src/Food Analyzer/FoodAnalyzer.API"
RUN dotnet build "FoodAnalyzer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodAnalyzer.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodAnalyzer.API.dll"]
