# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["Identity/Identity.Presentation/Identity.Presentation.csproj", "Identity/Identity.Presentation/"]
COPY ["Common/Common.Notification/Common.Notification.csproj", "Common/Common.Notification/"]
COPY ["Common/Common/Common.csproj", "Common/Common/"]
COPY ["Common/Common.Exceptions/Common.Exceptions.csproj", "Common/Common.Exceptions/"]
COPY ["Identity/Identity.Application/Identity.Application.csproj", "Identity/Identity.Application/"]
COPY ["Common/Common.DI/Common.DI.csproj", "Common/Common.DI/"]
COPY ["Identity/Identity.Contracts/Identity.Contracts.csproj", "Identity/Identity.Contracts/"]
COPY ["Common/Common.Repositories/Common.Repositories.csproj", "Common/Common.Repositories/"]
COPY ["Identity/Identity.Domain/Identity.Domain.csproj", "Identity/Identity.Domain/"]
COPY ["Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Identity/Identity.Infrastructure/"]
RUN dotnet restore "./Identity/Identity.Presentation/Identity.Presentation.csproj"
COPY . .
WORKDIR "/src/Identity/Identity.Presentation"
RUN dotnet build "./Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Presentation.dll"]