# Basic

Provides basic ressource management.

## Run the projects with docker-compose

### Create a https certificate

A certificate, based on the .net core dev-cert, will be generated for the project as pfx file.
This file will be used by the `Basic.WebApi` project to serve the API.

1. `dotnet dev-certs https -ep $env:APPDATA/ASP.NET/https/basic.pfx -p {my_password}`
1. Copy `.env.docker-compose` to `.env.docker-compose.local`
1. Update `.env.docker-compose.local` to set the password value

### Convert to pem for the front-end project

The generated certificate will be converted to pem/key format to be used by NGINX as part
of the `front` project.

**Step 1** - Connect to wsl or to any other openssl compatible prompt
1. `wsl -d Ubuntu`
1. `cd /mnt/XXX/AppData/Roaming/ASP.NET/Https`

**Step 2** - Convert the pfx file to pem/key format
1. `openssl pkcs12 -in basic.pfx -nocerts -out key.pem`
1. `openssl rsa -in key.pem -out basic.key`
1. `openssl pkcs12 -in basic.pfx -clcerts -nokeys -out basic.pem`

## Steps to add a new entity

### Add the model class and create the database structure

Key project: **Basic.DataAccess**

1. Define the Model class inside the Basic.Model project
	- Should be named according to business usage
	- Should inherit from the `BaseModel` class
1. Add any entity framework specificities inside the Context.OnModelCreation method
1. Run the command to add a migration `dotnet ef migrations add`
1. Review the migration code
1. Run the command to update the database `dotnet ef database update`

### Add the web api elements

Key project: **Basic.WebApi**

1. Create the DTO classes in the DTOs folder (`ForList`, `ForView`, `ForEdit`)
1. Create the controller in the Controllers folder
1. Add the automapper config for the new DTOs in the `MappingProfile` class
