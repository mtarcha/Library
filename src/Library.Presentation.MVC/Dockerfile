FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Library.Presentation.MVC/Library.Presentation.MVC.csproj", "src/Library.Presentation.MVC/"]
COPY ["src/Library.Domain/Library.Domain.csproj", "src/Library.Domain/"]
COPY ["src/Library.Domain.Common/Library.Domain.Common.csproj", "src/Library.Domain.Common/"]
COPY ["src/Library.Infrastructure.Data/Library.Infrastructure.Data.csproj", "src/Library.Infrastructure.Data/"]
RUN dotnet restore "src/Library.Presentation.MVC/Library.Presentation.MVC.csproj"
COPY . .
WORKDIR "/src/src/Library.Presentation.MVC"
RUN dotnet build "Library.Presentation.MVC.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Library.Presentation.MVC.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Library.Presentation.MVC.dll"]
