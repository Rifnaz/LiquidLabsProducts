# Use the official .NET SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*

# Copy csproj files and restore dependencies
COPY WebAPI/*.csproj ./WebAPI/
COPY DbLayer/*.csproj ./DbLayer/
COPY ServiceLayer/*.csproj ./ServiceLayer/
RUN dotnet restore WebAPI/WebAPI.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish WebAPI/WebAPI.csproj -c Release -o /app/publish

# Use runtime image for execution
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "WebAPI.dll"]
