FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Event/Event.Presentation/Event.Presentation.csproj", "Event/Event.Presentation/"]
COPY ["Core/Core.Api/Core.Api.csproj", "Core/Core.Api/"]
COPY ["Core/Core.Exceptions/Core.Exceptions.csproj", "Core/Core.Exceptions/"]
COPY ["Core/Core.Models/Core.Models.csproj", "Core/Core.Models/"]
COPY ["Event/Event.Infrastructure/Event.Infrastructure.csproj", "Event/Event.Infrastructure/"]
COPY ["Event/Event.Domain/Event.Domain.csproj", "Event/Event.Domain/"]
COPY ["Event/Event.Contracts/Event.Contracts.csproj", "Event/Event.Contracts/"]
COPY ["Common/Common.Refit/Common.Refit.csproj", "Common/Common.Refit/"]
COPY ["Event/Event.Application/Event.Application.csproj", "Event/Event.Application/"]
COPY ["Common/Common/Common.csproj", "Common/Common/"]
RUN dotnet restore "Event/Event.Presentation/Event.Presentation.csproj"
COPY . .
WORKDIR "/src/Event/Event.Presentation"
RUN dotnet build "Event.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Event.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Event.Presentation.dll"]
