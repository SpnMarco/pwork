# Backend Dockerfile for .NET 8.0 API

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Expose port (Render will set PORT env variable)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Create directory for SQLite database
RUN mkdir -p /app/data

# Run the application
ENTRYPOINT ["dotnet", "MedicalAppointments.dll"]
