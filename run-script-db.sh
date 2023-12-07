#!/bin/bash
# Set the SA password from the secret
export SA_PASSWORD=$(cat /run/secrets/db_password)
# Start SQL Server
exec /opt/mssql/bin/sqlservr