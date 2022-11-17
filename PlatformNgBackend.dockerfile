FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
ENTRYPOINT ["dotnet", "run"]
