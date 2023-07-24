FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 6001

ENV ASPNETCORE_URLS=https://+:6001

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DistributedDpApiDemo.csproj", "./"]
RUN dotnet restore "DistributedDpApiDemo.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DistributedDpApiDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DistributedDpApiDemo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DistributedDpApiDemo.dll"]
