# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln ./
COPY ProjectManagementSystem/*.csproj ./ProjectManagementSystem/
RUN dotnet restore

# Copy the rest of the source code
COPY ProjectManagementSystem/ ./ProjectManagementSystem/
WORKDIR /src/ProjectManagementSystem

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Use $PORT if provided, default to 8080
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

ENTRYPOINT ["dotnet", "ProjectManagementSystem.dll"]