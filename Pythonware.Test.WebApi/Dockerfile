FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Pythonware.Test.WebApi/Pythonware.Test.WebApi.csproj", "Pythonware.Test.WebApi/"]
RUN dotnet restore "Pythonware.Test.WebApi/Pythonware.Test.WebApi.csproj"
COPY . .
WORKDIR "/src/Pythonware.Test.WebApi"
RUN dotnet build "Pythonware.Test.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pythonware.Test.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pythonware.Test.WebApi.dll"]