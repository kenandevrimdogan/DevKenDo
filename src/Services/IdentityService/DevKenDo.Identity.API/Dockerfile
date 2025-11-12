# -------------------------
# 1. Base runtime
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Non-root user
ARG APP_UID=1000
USER $APP_UID

# -------------------------
# 2. Build
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Restore bağımlılıkları için önce .csproj dosyalarını kopyala
COPY ["src/Services/IdentityService/DevKenDo.Identity.API/DevKenDo.Identity.API.csproj", "src/Services/IdentityService/DevKenDo.Identity.API/"]
COPY ["src/Services/IdentityService/DevKenDo.Identity.Application/DevKenDo.Identity.Application.csproj", "src/Services/IdentityService/DevKenDo.Identity.Application/"]
COPY ["src/Services/IdentityService/DevKenDo.Identity.Domain/DevKenDo.Identity.Domain.csproj", "src/Services/IdentityService/DevKenDo.Identity.Domain/"]
COPY ["src/Services/IdentityService/DevKenDo.Identity.Infrastructure/DevKenDo.Identity.Infrastructure.csproj", "src/Services/IdentityService/DevKenDo.Identity.Infrastructure/"]

RUN dotnet restore "src/Services/IdentityService/DevKenDo.Identity.API/DevKenDo.Identity.API.csproj"

# Tüm kaynakları kopyala ve build/publish
COPY . .
WORKDIR "/src/src/Services/IdentityService/DevKenDo.Identity.API"
RUN dotnet build "DevKenDo.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# -------------------------
# 3. Publish
# -------------------------
FROM build AS publish
RUN dotnet publish "DevKenDo.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -------------------------
# 4. Final runtime image
# -------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevKenDo.Identity.API.dll"]
