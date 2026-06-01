FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Vcm.Api/Vcm.Api.csproj", "src/Vcm.Api/"]
COPY ["src/Vcm.Application/Vcm.Application.csproj", "src/Vcm.Application/"]
COPY ["src/Vcm.Domain/Vcm.Domain.csproj", "src/Vcm.Domain/"]
COPY ["src/Vcm.Infrastructure/Vcm.Infrastructure.csproj", "src/Vcm.Infrastructure/"]
RUN dotnet restore "src/Vcm.Api/Vcm.Api.csproj"
COPY . .
RUN dotnet publish "src/Vcm.Api/Vcm.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Vcm.Api.dll"]
