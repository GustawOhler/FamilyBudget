# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "./Backend/UI/FamilyBudgetUI.csproj"
# Build and publish a release
RUN dotnet publish "./Backend/UI/FamilyBudgetUI.csproj" -c Release -o out

RUN dotnet tool install --global dotnet-ef --version 7.*
ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet ef migrations bundle --project "./Backend/UI/FamilyBudgetUI.csproj" --output "./out/bundle.exe"

# Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/out .

EXPOSE 5001