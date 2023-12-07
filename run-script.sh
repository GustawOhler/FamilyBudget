#!/bin/bash
# Set connection string as env variables
DB_PASSWORD=`cat /run/secrets/db_password`
DB_CONNECTION="Server=sqldata,1433;Database=master;User id=SA;Password=$DB_PASSWORD;Trust Server Certificate=true;"
export ConnectionStrings_FamilyBudgetDatabase="$DB_CONNECTION"
# Set JWT Secret as env variable
export JWT_Secret=$(cat /run/secrets/jwt_secret)
# Perform migrations
./bundle.exe --connection "$DB_CONNECTION"
# Run web app
dotnet FamilyBudgetUI.dll --launch-profile "http-docker"