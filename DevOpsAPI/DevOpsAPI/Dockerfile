#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Add yandex cert
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS certAdd
WORKDIR /app
COPY ["DevOpsAPI/Cert", "Cert"]

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DevOpsAPI/DevOpsAPI.csproj", "DevOpsAPI/"]
RUN dotnet restore "DevOpsAPI/DevOpsAPI.csproj"
COPY . .
WORKDIR "/src/DevOpsAPI"
RUN dotnet build "DevOpsAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevOpsAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=certAdd /app/Cert /app/Cert
RUN mkdir -p Logs
RUN mkdir -p StaticFiles
ENTRYPOINT ["dotnet", "DevOpsAPI.dll"]