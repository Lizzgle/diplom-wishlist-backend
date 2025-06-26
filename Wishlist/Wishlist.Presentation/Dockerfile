FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wishlist/Wishlist.Presentation/Wishlist.Presentation.csproj", "Wishlist/Wishlist.Presentation/"]
COPY ["Wishlist/Wishlist.Application/Wishlist.Application.csproj", "Wishlist/Wishlist.Application/"]
COPY ["Wishlist/Wishlist.Contracts/Wishlist.Contracts.csproj", "Wishlist/Wishlist.Contracts/"]
COPY ["Wishlist/Wishlist.Domain/Wishlist.Domain.csproj", "Wishlist/Wishlist.Domain/"]
COPY ["Core/Core.Exceptions/Core.Exceptions.csproj", "Core/Core.Exceptions/"]
COPY ["Core/Core.Api/Core.Api.csproj", "Core/Core.Api/"]
COPY ["Core/Core.Models/Core.Models.csproj", "Core/Core.Models/"]
COPY ["Wishlist/Wishlist.Infrastructure/Wishlist.Infrastructure.csproj", "Wishlist/Wishlist.Infrastructure/"]
COPY ["Common/Common.Dropbox/Common.Dropbox.csproj", "Common/Common.Dropbox/"]
COPY ["Common/Common/Common.csproj", "Common/Common/"]
RUN dotnet restore "Wishlist/Wishlist.Presentation/Wishlist.Presentation.csproj"
COPY . .
WORKDIR "/src/Wishlist/Wishlist.Presentation"
RUN dotnet build "Wishlist.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Wishlist.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wishlist.Presentation.dll"]
