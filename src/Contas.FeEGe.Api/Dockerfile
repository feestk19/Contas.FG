# Multi-stage Dockerfile for Contas.FeEGe.Api (.NET 8)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Contas.FeEGe.Api/Contas.FeEGe.Api.csproj", "src/Contas.FeEGe.Api/"]
COPY ["src/Contas.FeEGe.Application/Contas.FeEGe.Application.csproj", "src/Contas.FeEGe.Application/"]
COPY ["src/Contas.FeEGe.Domain/Contas.FeEGe.Domain.csproj", "src/Contas.FeEGe.Domain/"]
COPY ["src/Contas.FeEGe.Infrastructure/Contas.FeEGe.Infrastructure.csproj", "src/Contas.FeEGe.Infrastructure/"]

RUN dotnet restore "src/Contas.FeEGe.Api/Contas.FeEGe.Api.csproj"
COPY . .
RUN dotnet publish "src/Contas.FeEGe.Api/Contas.FeEGe.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Contas.FeEGe.Api.dll"]
