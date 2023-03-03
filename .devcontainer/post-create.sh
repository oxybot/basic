#!/bin/sh

# load dependencies
cd front && yarn
cd -
dotnet restore

# setup https
chmod +x setup-https.sh
./setup-https.sh

# update configuration for front
echo "REACT_APP_API_ROOT_URL=https://${CODESPACE_NAME}-7268.preview.app.github.dev" >> front/.env.local

# update configuration for API
dotnet user-secrets set "BaseUrl" "https://${CODESPACE_NAME}-7268.preview.app.github.dev" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "Cors:Origins" "https://${CODESPACE_NAME}-3000.preview.app.github.dev,https://${CODESPACE_NAME}-7268.preview.app.github.dev" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "ActiveDirectory:Server" "ldap" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "EmailService:Server" "smtp" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "EmailService:FrontBaseUrl" "https://${CODESPACE_NAME}-3000.preview.app.github.dev" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "DatabaseDriver" "MySql" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "ConnectionStrings:MySql" "Server=mysql;Database=basic;User=basic;Password=basic;Pooling=False;" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "ConnectionStrings:SqlServer" "Server=mssql;Database=basic;User=basic;Password=basic@Passw0rd;Trusted_Connection=True;MultipleActiveResultSets=true" --project "/workspaces/basic/src/Basic.WebApi"
