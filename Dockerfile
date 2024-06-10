FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory and copy the project files
WORKDIR /src
COPY ["EasyPayChallenge.WebApi/EasyPayChallenge.WebApi.csproj", "EasyPayChallenge.WebApi/"]
COPY ["EasyPayChallenge.Infrastructure/EasyPayChallenge.Infrastructure.csproj", "EasyPayChallenge.Infrastructure/"]
COPY ["EasyPayChallenge.Application/EasyPayChallenge.Application.csproj", "EasyPayChallenge.Application/"]
COPY ["EasyPayChallenge.Domain/EasyPayChallenge.Domain.csproj", "EasyPayChallenge.Domain/"]
RUN dotnet restore "EasyPayChallenge.WebApi/EasyPayChallenge.WebApi.csproj"

COPY . .
WORKDIR "/src/EasyPayChallenge.WebApi"
RUN dotnet publish "EasyPayChallenge.WebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# Install libicu package
RUN apt-get update \
    && apt-get install -y libicu-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Set the working directory and expose port 80
WORKDIR /app
EXPOSE 80

# Copy the published app from the build stage to the runtime stage
COPY --from=build /app/publish .

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "EasyPayChallenge.WebApi.dll"]
