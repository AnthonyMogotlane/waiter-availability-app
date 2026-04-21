# .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and project files
COPY ["WaiterAvailabilityApp.sln", "./"]
COPY ["WaiterAvailabilityApp.Common/WaiterAvailabilityApp.csproj", "WaiterAvailabilityApp.Common/"]
COPY ["WaiterAvailabilityApp.Web/WaiterAvailabilityAppRazor.csproj", "WaiterAvailabilityApp.Web/"]
COPY ["WaiterAvailabilityApp.Tests/WaiterAvailabilityApp.Tests.csproj", "WaiterAvailabilityApp.Tests/"]

# Restoring dependencies
RUN dotnet restore

# Source code copy
COPY . .

# Build and publish
WORKDIR "/src/WaiterAvailabilityApp.Web"
RUN dotnet publish -c Release -o /app/publish

# ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Publishing app from the build stage
COPY --from=build /app/publish .

# Exposing the port the app runs on
EXPOSE 80

# Setting the entry point
ENTRYPOINT ["dotnet", "WaiterAvailabilityAppRazor.dll"]