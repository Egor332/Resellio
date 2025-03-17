# Use the .NET SDK for development
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory inside the container
WORKDIR /app

# Copy the solution file
COPY *.sln ./

# Copy only the necessary project files
COPY ./ ./NotificationService

# Remove the test project reference from the solution file
RUN sed -i '/NotificationServiceTests.csproj/d' /app/NotificationService/NotificationService.sln

# Restore the .NET projects
RUN dotnet restore ./NotificationService/NotificationService.csproj

# Set environment to Development
ENV ASPNETCORE_ENVIRONMENT=Development

# Expose the port for accessing the API
EXPOSE 81

# Start the application in watch mode
CMD ["dotnet", "watch", "run", "--project", "NotificationService", "--urls", "http://0.0.0.0:80"]