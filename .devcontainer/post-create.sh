#!/bin/sh

# add to safe repository
git config --global --add safe.directory ${PWD}

# load dependencies
cd front && yarn
cd -
dotnet restore

# setup https
chmod +x setup-https.sh
./setup-https.sh

# update configuration for front
if [ "${CODESPACES}" = "true" ]; then
  echo "REACT_APP_API_ROOT_URL=https://${CODESPACE_NAME}-7268.preview.app.github.dev" >> front/.env.local
fi

# update configuration for API
if [ "${CODESPACES}" = "true" ]; then
  dotnet user-secrets set "BaseUrl" "https://${CODESPACE_NAME}-7268.preview.app.github.dev" --project "/workspaces/basic/src/Basic.WebApi"
  dotnet user-secrets set "Cors:Origins" "https://${CODESPACE_NAME}-3000.preview.app.github.dev,https://${CODESPACE_NAME}-7268.preview.app.github.dev" --project "/workspaces/basic/src/Basic.WebApi"
  dotnet user-secrets set "EmailService:FrontBaseUrl" "https://${CODESPACE_NAME}-3000.preview.app.github.dev" --project "/workspaces/basic/src/Basic.WebApi"
fi

# update configuration
dotnet user-secrets set "ActiveDirectory:Server" "ldap" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "EmailService:Server" "smtp" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "DatabaseDriver" "MySql" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "ConnectionStrings:MySql" "Server=mysql;Database=basic;User=basic;Password=basic;Pooling=False;" --project "/workspaces/basic/src/Basic.WebApi" 1>/dev/null
dotnet user-secrets set "ConnectionStrings:SqlServer" "Server=mssql;Database=basic;User=basic;Password=basic@Passw0rd;Trusted_Connection=True;MultipleActiveResultSets=true" --project "/workspaces/basic/src/Basic.WebApi" 1>/dev/null
dotnet user-secrets set "ConnectionStrings:MySql" "Server=mysql;Database=basic-test;User=basic-test;Password=basic-test;Pooling=False;" --project "/workspaces/basic/test/Basic.WebApi-Test" 1>/dev/null
dotnet user-secrets set "ConnectionStrings:SqlServer" "Server=mssql;Database=basic-test;User=basic;Password=basic@Passw0rd;Trusted_Connection=True;MultipleActiveResultSets=true" --project "/workspaces/basic/test/Basic.WebApi-Test" 1>/dev/null
