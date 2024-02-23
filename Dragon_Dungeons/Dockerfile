# Use the .NET Core SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory inside the container
WORKDIR /app

# Copy the dotnet project files to the container
COPY . ./

# Restore the .NET dependencies
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o out

# Create a new image using a lighter .NET Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the build-env to the new container
COPY --from=build-env /app/out .

# Expose the port your application listens on
EXPOSE 7047

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Dragon_Dungeons.dll"]