#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS="https://+;http://+"

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./Basic.WebApi/Basic.WebApi.csproj", "./Basic.WebApi/"]
COPY ["./Basic.Model/Basic.Model.csproj", "./Basic.Model/"]
COPY ["./Basic.DataAccess/Basic.DataAccess.csproj", "./Basic.DataAccess/"]
COPY ["./Basic.DataAccess.MySql/Basic.DataAccess.MySql.csproj", "./Basic.DataAccess.MySql/"]
COPY ["./Basic.DataAccess.SqlServer/Basic.DataAccess.SqlServer.csproj", "./Basic.DataAccess.SqlServer/"]
RUN dotnet restore "./Basic.WebApi/Basic.WebApi.csproj"
COPY . .
WORKDIR "/src/Basic.WebApi"
RUN dotnet build "Basic.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basic.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basic.WebApi.dll"]
