#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["API Gateway/YARP.LoadBalancer/YARP.LoadBalancer.csproj", "API Gateway/YARP.LoadBalancer/"]
RUN dotnet restore "API Gateway/YARP.LoadBalancer/YARP.LoadBalancer.csproj"
COPY . .
WORKDIR "/src/API Gateway/YARP.LoadBalancer"
RUN dotnet build "YARP.LoadBalancer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YARP.LoadBalancer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YARP.LoadBalancer.dll"]