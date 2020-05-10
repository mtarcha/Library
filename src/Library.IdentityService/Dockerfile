FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Library.IdentityService/Library.IdentityService.csproj", "src/Library.IdentityService/"]
RUN dotnet restore "src/Library.IdentityService/Library.IdentityService.csproj"
COPY . .
WORKDIR "/src/src/Library.IdentityService"
RUN dotnet build "Library.IdentityService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Library.IdentityService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Library.IdentityService.dll"]