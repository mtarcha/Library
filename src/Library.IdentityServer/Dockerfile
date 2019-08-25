FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Lubrary.IdentityServer/Library.IdentityServer.csproj", "Library.IdentityServer/"]
RUN dotnet restore "Lubrary.IdentityServer/Library.IdentityServer.csproj"
COPY . .
WORKDIR "/src/Library.IdentityServer"
RUN dotnet build "Library.IdentityServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Library.IdentityServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Library.IdentityServer.dll"]