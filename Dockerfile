FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["TechGearAPI.csproj", "./"]
RUN dotnet restore "TechGearAPI.csproj"
COPY . .
RUN dotnet build "TechGearAPI.csproj" -c Release -o /app/build
RUN dotnet publish "TechGearAPI.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "TechGearAPI.dll"]