
# Build stage
2
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
3
WORKDIR /src
4
​
5
# Copy solution and project files
6
COPY *.sln ./
7
COPY ProjectManagementSystem/*.csproj ./ProjectManagementSystem/
8
RUN dotnet restore
9
​
10
# Copy the rest of the source code
11
COPY ProjectManagementSystem/ ./ProjectanagementSystem/
12
WORKDIR /src/ProjectManagementSystem
13
​
14
# Publish the application
15
RUN dotnet publish -c Release -o /app/publish
16
​
17
# Runtime stage
18
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
19
WORKDIR /app
20
COPY --from=build /app/publish .
21
​
22
# Use $PORT if provided, default to 8080
23
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
24
EXPOSE 8080
25
​
26
ENTRYPOINT ["dotnet", "ProjectManagementSystem.dll"]
27




