#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["User/User.API/User.API.csproj", "User/User.API/"]
COPY ["User/User.Application/User.Application.csproj", "User/User.Application/"]
COPY ["User/UserData.Infrastructure/UserData.Infrastructure.csproj", "User/UserData.Infrastructure/"]
RUN dotnet restore "User/User.API/User.API.csproj"
COPY . .
WORKDIR "/src/User/User.API"
RUN dotnet build "User.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "User.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.API.dll"]
