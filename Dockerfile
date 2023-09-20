#DockerFile
# Use the .NET 6 SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory
WORKDIR /app

# Copy your .csproj and .sln files and restore dependencies
COPY MuonRoiSocialNetwork/*.csproj ./
# Copy the rest of your application code
COPY . ./

# Build the application with the Release configuration
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port your application listens on
EXPOSE 80

# Define the entry point for your application
ENTRYPOINT ["dotnet", "MuonRoiSocialNetwork.dll"]


