FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Identity/Identity.Presentation/Identity.Presentation.csproj", "Identity/Identity.Presentation/"]
COPY ["Common/Common.Notification/Common.Notification.csproj", "Common/Common.Notification/"]
COPY ["Common/Common/Common.csproj", "Common/Common/"]
COPY ["Identity/Identity.Application/Identity.Application.csproj", "Identity/Identity.Application/"]
COPY ["Core/Core.Exceptions/Core.Exceptions.csproj", "Core/Core.Exceptions/"]
COPY ["Identity/Identity.Contracts/Identity.Contracts.csproj", "Identity/Identity.Contracts/"]
COPY ["Identity/Identity.Domain/Identity.Domain.csproj", "Identity/Identity.Domain/"]
COPY ["Core/Core.Models/Core.Models.csproj", "Core/Core.Models/"]
COPY ["Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Identity/Identity.Infrastructure/"]
COPY ["Core/Core.Api/Core.Api.csproj", "Core/Core.Api/"]
RUN dotnet restore "Identity/Identity.Presentation/Identity.Presentation.csproj"
COPY . .
WORKDIR "/src/Identity/Identity.Presentation"
RUN dotnet build "Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Presentation.dll"]
