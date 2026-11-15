
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project files
COPY ["Data-Access-Layer/Data-Access-Layer.csproj", "Data-Access-Layer/"]
COPY ["Business-Logic-Layer/Business-Logic-Layer.csproj", "Business-Logic-Layer/"]
COPY ["Presentation-Logic-Layer/Presentation-Logic-Layer.csproj", "Presentation-Logic-Layer/"]

# Restore API project and its dependencies
RUN dotnet restore "Presentation-Logic-Layer/Presentation-Logic-Layer.csproj"

# Copy source code
COPY . .

# Publish API
WORKDIR /src/Presentation-Logic-Layer

RUN dotnet publish "Presentation-Logic-Layer.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Presentation-Logic-Layer.dll"]
