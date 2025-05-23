FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SecretSanta/SecretSanta.Presentation/SecretSanta.Presentation.csproj", "SecretSanta/SecretSanta.Presentation/"]
RUN dotnet restore "SecretSanta/SecretSanta.Presentation/SecretSanta.Presentation.csproj"
COPY . .
WORKDIR "/src/SecretSanta/SecretSanta.Presentation"
RUN dotnet build "SecretSanta.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SecretSanta.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecretSanta.Presentation.dll"]
