# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "./FamilyBudget.Backend/FamilyBudget.csproj"
# Build and publish a release
RUN dotnet publish "./FamilyBudget.Backend/FamilyBudget.csproj" -c Release -o out

RUN dotnet user-secrets init --project "./FamilyBudget.Backend/FamilyBudget.csproj"
RUN dotnet user-secrets set "JWT:Secret" "tC3byUseoajbwB69b0vsdJnRjsbTK7zC" --project "./FamilyBudget.Backend/FamilyBudget.csproj"
RUN dotnet user-secrets set "ConnectionStrings:FamilyBudgetDatabase" "Server=localhost:1433;Database=master;User id=SA;Password=testing_password;Trust Server Certificate=true;" --project "./FamilyBudget.Backend/FamilyBudget.csproj"

# Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/out .

# EXPOSE 5001

# ENTRYPOINT ["dotnet", "FamilyBudget.dll"]
