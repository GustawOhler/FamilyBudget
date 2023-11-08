# FamilyBudget

This project is mainly API for managing group budgets. You can create budget, add your friends or family and add various incomes and expenses to get clear view of it. Frontend part of application is in progress.

## Usage
Unfortunately contanerization of the project has not been fully done yet, thus only manual setup is available. To run project you need to have SQL Server with FamilyBudget database created.

First you have to clone source code to designed folder. Then you need to run commands to restore packages, build solution and add important secret variables:
```
dotnet restore
dotnet build
dotnet user-secrets init --project "./FamilyBudget.Backend/FamilyBudget.csproj"
dotnet user-secrets set "JWT:Secret" "!!YourChosenSecretPhrase!!" --project "./FamilyBudget.Backend/FamilyBudget.csproj"
dotnet user-secrets set "ConnectionStrings:FamilyBudgetDatabase" "Server=localhost;Database=FamilyBudget;Trusted_Connection=True;Trust Server Certificate=true;" --project "./FamilyBudget.Backend/FamilyBudget.csproj"
dotnet dev-certs https --trust
```

Next you need to run migrations to have database in correct state:
```
dotnet ef database update
```

And lastly you need to run application:
```
dotnet run --launch-profile "https"
```