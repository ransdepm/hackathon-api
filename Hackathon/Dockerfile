FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

EXPOSE 7000
EXPOSE 7001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Hackathon.Service/Hackathon.Service.csproj", "Hackathon.Service/"]
COPY ["Hackathon.Data/Hackathon.Data.csproj", "Hackathon.Data/"]
RUN dotnet restore "Hackathon.Service/Hackathon.Service.csproj"

COPY . .
WORKDIR "/src/Hackathon.Service"
RUN dotnet build "Hackathon.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hackathon.Service.csproj" -c Release -o /app/publish
COPY ["Hackathon.Service/Hackathon.Service.xml", "/app/publish"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Hackathon.Service.dll"]
