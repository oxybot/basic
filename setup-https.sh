#!/bin/sh

# create local dev certificate
dotnet dev-certs https -ep .keys/basic.pfx -p password
dotnet dev-certs https -ep .keys/basic.pem --format pem --no-password

# create configuration for front
echo "SSL_CRT_FILE=${PWD}/.keys/basic.pem" > front/.env.local
echo "SSL_KEY_FILE=${PWD}/.keys/basic.key" >> front/.env.local

# create configuration for API
dotnet user-secrets set "Kestrel:Certificates:Default:Path" "${PWD}/.keys/basic.pfx" --project "/workspaces/basic/src/Basic.WebApi"
dotnet user-secrets set "Kestrel:Certificates:Default:Password" "password" --project "/workspaces/basic/src/Basic.WebApi"
