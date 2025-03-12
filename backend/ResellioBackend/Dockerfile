# Use the .NET SDK for development
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet tool install --global dotnet-ef

# Add the .NET tools directory to the PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

# Set working directory inside the container
WORKDIR /app

# Copy the solution file
COPY *.sln ./

# Copy only the necessary project files
COPY ./ ./ResellioBackend

# Remove the test project reference from the solution file
RUN sed -i '/ResellioBackendTests.csproj/d' /app/ResellioBackend/ResellioBackend.sln

# Restore the .NET projects
RUN dotnet restore ./ResellioBackend/ResellioBackend.csproj

# Set environment to Development
ENV ASPNETCORE_ENVIRONMENT=Development

# Expose the port for accessing the API
EXPOSE 80

# Verify installation of dotnet-ef
RUN dotnet --list-sdks
RUN dotnet --list-runtimes
RUN dotnet tool list --global  # This should list dotnet-ef

# Start the application in watch mode
CMD ["dotnet", "watch", "run", "--project", "ResellioBackend", "--urls", "http://0.0.0.0:80"]
