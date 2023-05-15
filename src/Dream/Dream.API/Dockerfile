#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Dream/Dream.WebApi/Dream.API.csproj", "Dream/Dream.WebApi/"]
COPY ["Dream/Dream.Infrastructure/Dream.Infrastructure.csproj", "Dream/Dream.Infrastructure/"]
COPY ["Dream/Dream.Domain/Dream.Domain.csproj", "Dream/Dream.Domain/"]
COPY ["Core/Core.SharedKernel/Core.SharedKernel.csproj", "Core/Core.SharedKernel/"]
RUN dotnet restore "Dream/Dream.WebApi/Dream.API.csproj"
COPY . .
WORKDIR "/src/Dream/Dream.WebApi"
RUN dotnet build "Dream.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dream.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dream.API.dll"]