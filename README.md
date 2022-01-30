# Basic

Provides basic ressource management.

## Run the projects with docker-compose

### Create a https certificate

1. `dotnet dev-certs https -ep $env:APPDATA/ASP.NET/https/basic.pfx -p {my_password}`
1. Copy `.env.docker-compose` to `.env.docker-compose.local`
1. Update `.env.docker-compose.local` to set the password value

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

1. Create the DTO classes in the DTOs folder
1. Create the controller in the Controllers folder
1. Add the automapper config for the new DTOs in the MappingProfile class
