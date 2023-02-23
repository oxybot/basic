#!/bin/sh

# load dependencies
cd front && yarn
cd -
dotnet restore

# create local dev certificate
dotnet dev-certs https -ep .keys/basic.pfx -p password
dotnet dev-certs https -ep .keys/basic.pem --format pem --no-password

# create devcontainer configuration for front
echo "SSL_CRT_FILE=/workspaces/basic/.keys/basic.pem" > front/.env.local
echo "SSL_KEY_FILE=/workspaces/basic/.keys/basic.key" >> front/.env.local
echo "REACT_APP_API_ROOT_URL=https://${CODESPACE_NAME}-7268.preview.app.github.dev" >> front/.env.local

# create devcontainer configuration for API
dotnet user-secrets set "BaseUrl" "https://${CODESPACE_NAME}-7268.preview.app.github.dev" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "Cors:Origins" "https://${CODESPACE_NAME}-3000.preview.app.github.dev,https://${CODESPACE_NAME}-7268.preview.app.github.dev" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "EmailService:FrontBaseUrl" "https://${CODESPACE_NAME}-3000.preview.app.github.dev" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "Kestrel:Certificates:Default:Path" "/workspaces/basic/.keys/basic.pfx" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "Kestrel:Certificates:Default:Password" "password" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "DatabaseDriver" "MySql" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "ConnectionStrings:MySql" "Server=mysql;Database=basic;User=basic;Password=basic;Pooling=False;" --project "/workspaces/basic/src/Basic.WebApi"
