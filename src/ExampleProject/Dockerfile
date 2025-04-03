FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ExampleProject/ExampleProject.csproj", "src/ExampleProject/"]
COPY ["src/RateLimiter/RateLimiter.csproj", "src/RateLimiter/"]
RUN dotnet restore "src/ExampleProject/ExampleProject.csproj"
RUN dotnet restore "src/RateLimiter/RateLimiter.csproj"
COPY . .
WORKDIR "/src/src/ExampleProject"
RUN dotnet build "ExampleProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExampleProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExampleProject.dll"]