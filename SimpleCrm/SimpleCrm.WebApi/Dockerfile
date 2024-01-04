#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimpleCrm.WebApi/SimpleCrm.WebApi.csproj", "SimpleCrm.WebApi/"]
RUN dotnet restore "SimpleCrm.WebApi/SimpleCrm.WebApi.csproj"
COPY . .
WORKDIR "/src/SimpleCrm.WebApi"
RUN dotnet build "SimpleCrm.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleCrm.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleCrm.WebApi.dll"]