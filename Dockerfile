FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EnergomeraTestTask.csproj", "./"]
RUN dotnet restore "EnergomeraTestTask.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "EnergomeraTestTask.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "EnergomeraTestTask.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Kml ./Kml
ENTRYPOINT ["dotnet", "EnergomeraTestTask.dll", "--urls", "http://0.0.0.0:8080"]
