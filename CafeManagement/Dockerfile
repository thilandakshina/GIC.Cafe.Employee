# Base image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first
COPY ["Cafe.API/Cafe.API.csproj", "./Cafe.API/"]
COPY ["Cafe.Application/Cafe.Application.csproj", "./Cafe.Application/"]
COPY ["Cafe.Domain/Cafe.Domain.csproj", "./Cafe.Domain/"]
COPY ["Cafe.Infrastructure/Cafe.Infrastructure.csproj", "./Cafe.Infrastructure/"]

# Copy the rest of the files
COPY . .

# Restore and build API project
RUN dotnet restore "Cafe.API/Cafe.API.csproj"
RUN dotnet build "Cafe.API/Cafe.API.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "Cafe.API/Cafe.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Cafe.API.dll"]