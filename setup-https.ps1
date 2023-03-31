# create local dev certificate
dotnet dev-certs https -ep .keys/basic.pfx -p password
dotnet dev-certs https -ep .keys/basic.pem --format pem --no-password

# create configuration for front
"SSL_CRT_FILE=${PWD}/.keys/basic.pem" > front/.env.local
"SSL_KEY_FILE=${PWD}/.keys/basic.key" >> front/.env.local

# create configuration for API
dotnet user-secrets set "Kestrel:Certificates:Default:Path" "${PWD}/.keys/basic.pfx" --project "src/Basic.WebApi"
dotnet user-secrets set "Kestrel:Certificates:Default:Password" "password" --project "src/Basic.WebApi" | Out-Null
